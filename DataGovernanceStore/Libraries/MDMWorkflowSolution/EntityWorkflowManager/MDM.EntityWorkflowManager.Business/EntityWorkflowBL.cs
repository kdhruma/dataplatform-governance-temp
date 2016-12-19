using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MDM.EntityWorkflowManager.Business
{
    using Core.Extensions;
    using MDM.AdminManager.Business;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Workflow;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.EntityManager.Business;
    using MDM.ExceptionManager;
    using MDM.Interfaces;
    using MDM.JigsawIntegrationManager;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.LookupManager.Business;
    using MDM.MessageBrokerManager;
    using MDM.MessageManager.Business;
    using MDM.Utility;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.Workflow.Utility;
    using MDM.WorkflowRuntimeEngine;
    using Newtonsoft.Json.Linq;
    using MDMBOW = MDM.BusinessObjects.Workflow;

    /// <summary>
    /// 
    /// </summary>
    public class EntityWorkflowBL : BusinessLogicBase, IEntityWorkflowManager
    {
        #region Fields

        /// <summary>
        /// Field denoting locale Message
        /// </summary>
        private LocaleMessage _localeMessage = null;

        /// <summary>
        /// Field denoting localeMessageBL
        /// </summary>
        private LocaleMessageBL _localeMessageBL = new LocaleMessageBL();

        /// <summary>
        /// 
        /// </summary>
        private IEntityManager _entityManager = null;

        /// <summary>
        /// The current trace settings
        /// </summary>
        private TraceSettings _currentTraceSettings = null;

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;

        private const String WorkflowAction_Start = "Start";

        private const String WorkflowAction_Resume = "Resume";

        private const String STATEVIEW_WORKFLOW_DEPENDENCY = "Stateview_Workflow_Dependency";

        private const String STATEVIEW = "Stateview";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default Constructor that loads the security principal from Cache if present
        /// </summary>
        public EntityWorkflowBL(IEntityManager entityManager)
        {
            this._entityManager = entityManager;
            this._currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            //GetSecurityPrincipal();
            this._securityPrincipal = new SecurityPrincipal(new UserIdentity("cfadmin", "cfadmin"));
        }

        #endregion Constructors

        #region Methods

        #region Start Workflow

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow1");
                diagnosticActivity.Start();
            }

            EntityOperationResult entityOperationResult = null;

            try
            {
                #region Validation

                if (entityId <= 0)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                    throw new MDMOperationException("111795", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//EntityId must be greater than 0
                }

                #endregion Validation

                EntityOperationResultCollection entityOperationResultCollection = this.StartWorkflow(new Collection<Int64>() { entityId }, workflowName, workflowVersionId, serviceType, serviceId, callerContext, comments);

                if (entityOperationResultCollection != null && entityOperationResultCollection.Count > 0)
                {
                    entityOperationResult = entityOperationResultCollection.FirstOrDefault();
                }
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

            return entityOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>diagnosticActivity.LogInformation(
        public EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow2");
                diagnosticActivity.Start();
            }

            #region Validation

            if (entityIdList == null || entityIdList.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111847", false, callerContext);
                throw new MDMOperationException("111847", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//EntityIdList cannot be null or empty.
            }

            #endregion Validation

            EntityOperationResultCollection entityORC = null;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Param : EntityIds to invoke workflow for : " + ValueTypeHelper.JoinCollection(entityIdList, ","));
                    diagnosticActivity.LogInformation("Param : WorkflowName : " + workflowName);
                    diagnosticActivity.LogInformation("Param : WorkflowVersionId : " + workflowVersionId);
                }

                #region Replaced GetEntities() currently with new EntityCollection

                // EntityCollection is needed here to prepare OperationResultSchema using entity.RefferenceId, entity.LongName, etc.
                EntityCollection entities = new EntityCollection(); // #Dhruma
                foreach (Int64 id in entityIdList)
                {
                    Entity entity = new Entity() { Id = id };
                    entities.Add(entity);
                }

                #endregion

                if (entities != null)
                {
                    entityORC = this.StartWorkflow(entities, workflowName, workflowVersionId, serviceType, serviceId, WorkflowInstanceRunOptions.RunAsMultipleInstances, callerContext, comments);
                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                        diagnosticActivity.LogInformation("No entities found to invoke workflow");
                }
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

            return entityORC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow3");
                diagnosticActivity.Start();
            }


            #region Validation

            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow", MDMTraceSource.AdvancedWorkflow);//Entity is null
            }

            #endregion Validation

            EntityOperationResult entityOperationResult = null;

            try
            {
                EntityOperationResultCollection entityOperationResultCollection = this.StartWorkflow(new EntityCollection() { entity }, workflowName, workflowVersionId, serviceType, serviceId, WorkflowInstanceRunOptions.RunAsMultipleInstances, callerContext, comments);

                if (entityOperationResultCollection != null && entityOperationResultCollection.Count >= 1)
                {
                    entityOperationResult = entityOperationResultCollection.FirstOrDefault();
                }
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

            return entityOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow4");
                diagnosticActivity.Start();
            }

            #region Validation

            if (entities == null || entities.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
                throw new MDMOperationException("111816", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//Entities collection is null or empty
            }

            #endregion Validation

            EntityOperationResultCollection entityORC = null;


            try
            {
                entityORC = this.StartWorkflow(entities, workflowName, workflowVersionId, serviceType, serviceId, WorkflowInstanceRunOptions.RunAsMultipleInstances, callerContext, comments);

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
            return entityORC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="workflowInstanceRunOption"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResult StartWorkflow(Int64 entityId, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow5");
                diagnosticActivity.Start();
            }

            #region Validation

            if (entityId <= 0)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111795", false, callerContext);
                throw new MDMOperationException("111795", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//EntityId must be greater than 0
            }

            #endregion Validation

            EntityOperationResult entityOperationResult = null;

            try
            {
                EntityOperationResultCollection entityORC = this.StartWorkflow(new Collection<Int64> { entityId }, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

                if (entityORC != null && entityORC.Count >= 1)
                {
                    entityOperationResult = entityORC.FirstOrDefault();
                }
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

            return entityOperationResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="workflowInstanceRunOption"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResultCollection StartWorkflow(Collection<Int64> entityIds, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            DiagnosticActivity diagnosticActivity = null;
            EntityOperationResultCollection entityORC = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow6");
                diagnosticActivity.Start();
            }

            try
            {
                #region Validation

                if (entityIds == null || entityIds.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111847", false, callerContext);
                    throw new MDMOperationException("111847", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//EntityIdList cannot be null or empty.
                }

                #endregion Validation

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.LogInformation("Param : EntityIds to invoke workflow for : " + ValueTypeHelper.JoinCollection(entityIds, ","));
                    diagnosticActivity.LogInformation("Param : WorkflowName : " + workflowName);
                    diagnosticActivity.LogInformation("Param : WorkflowVersionId : " + workflowVersionId);
                }

                EntityCollection entities = GetEntities(entityIds, callerContext);

                if (entities != null)
                {



                    entityORC = this.StartWorkflow(entities, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);


                }
                else
                {
                    if (_currentTraceSettings.IsBasicTracingEnabled)
                        diagnosticActivity.LogInformation("No entities found to invoke workflow");
                }

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

            return entityORC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceId"></param>
        /// <param name="workflowInstanceRunOption"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        public EntityOperationResult StartWorkflow(Entity entity, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            #region Validation

            if (entity == null)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111815", false, callerContext);
                throw new MDMOperationException("111815", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//Entity is null
            }

            #endregion Validation

            EntityOperationResult entityOperationResult = null;

            EntityOperationResultCollection entityORC = null;
            DiagnosticActivity diagnosticActivity = null;

            try
            {
                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow7");
                    diagnosticActivity.Start();
                }

                entityORC = this.StartWorkflow(new EntityCollection() { entity }, workflowName, workflowVersionId, serviceType, serviceId, workflowInstanceRunOption, callerContext, comments);

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
            if (entityORC != null && entityORC.Count >= 1)
            {
                entityOperationResult = entityORC.FirstOrDefault();
            }

            return entityOperationResult;
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
        public EntityOperationResultCollection StartWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String serviceType, Int32 serviceId, WorkflowInstanceRunOptions workflowInstanceRunOption, CallerContext callerContext, String comments = "")
        {
            #region Validation

            if (entities == null || entities.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
                throw new MDMOperationException("111816", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//Entities collection is null or empty
            }

            #endregion Validation

            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow8");
                diagnosticActivity.Start();
            }

            EntityOperationResultCollection entityORC = new EntityOperationResultCollection();
            //Create WorkflowDataContext before calling core WorkflowService to start the workflow.
            try
            {
                MDMBOW.WorkflowDataContext workflowDataContext = new MDMBOW.WorkflowDataContext();

                workflowDataContext.MDMObjectCollection = this.GetWorkflowMDMObjectCollection(entities);
                //workflowDataContext.MDMObjectCollection = this.GetWorkflowMDMObjectCollection(entityIds); // #Dhruma
                workflowDataContext.Module = callerContext.Module;
                workflowDataContext.Application = callerContext.Application;
                workflowDataContext.WorkflowName = workflowName;
                workflowDataContext.WorkflowVersionId = workflowVersionId;
                workflowDataContext.WorkflowComments = comments;

                String loggedInUser = String.Empty;

                //if (_securityPrincipal != null) // #Dhruma
                //{
                //    loggedInUser = _securityPrincipal.CurrentUserName;
                //}

                WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
                OperationResult operationResult = new OperationResult();



                //Prepare EntityOperationResultCollection for incoming entities.
                entityORC = Utility.PrepareEntityOperationResultsSchema(entities);

                operationResult = workflowRuntimeBL.StartWorkflow(workflowDataContext, serviceType, serviceId, loggedInUser, workflowInstanceRunOption, entityORC, callerContext);
                Int32 invokeSuccessCount = operationResult.ReturnValues.Count > 0 ? ValueTypeHelper.Int32TryParse(operationResult.ReturnValues.FirstOrDefault().ToString(), 0) : 0;

                UpdateEntityOperationResultCollection(entityORC, operationResult, invokeSuccessCount, WorkflowAction_Start, callerContext);

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

            return entityORC;
        }
     
        #region Rest APIs

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestJsonObject"></param>
        /// <returns></returns>
        public JObject StartWorkflow(JObject requestJsonObject)
        {
            JObject responseJsonObject = null;
            DiagnosticActivity diagnosticActivity = null;
            
            String commentsIfAny = String.Empty;
            String status = "Success";
            responseJsonObject = new JObject();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow8");
                diagnosticActivity.Start();
            }

            try
            {
                #region Read input json

                Boolean returnRequest = ValueTypeHelper.BooleanTryParse(requestJsonObject["returnRequest"].ToString(), false);
                String entityGUID = requestJsonObject["dataObjects"][0]["id"].ToString();
                String workflowName = requestJsonObject["requestParams"]["workflowName"].ToString();
                CallerContext callerContext = requestJsonObject["requestParams"]["callerContext"].ConvertToObject<CallerContext>();

                JToken commentsJtoken = requestJsonObject["requestParams"]["comments"];
                if (commentsJtoken != null)
                {
                    commentsIfAny = commentsJtoken.ToString();
                }

                #endregion Read input json

                OperationResult operationResult = this.StartWorkflow(entityGUID, workflowName, -1, callerContext, commentsIfAny);

                #region Prepare JSON output

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    status = "Failure";
                }

                if (returnRequest)
                {
                    responseJsonObject.Add(new JProperty("dataObjectOperationRequest", requestJsonObject));
                }

                responseJsonObject.Add("dataObjectOperationResponse", new JObject(new JProperty("status", status)));

                #endregion Prepare JSON output
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

            return responseJsonObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestJsonObject"></param>
        /// <returns></returns>
        public JObject ResumeWorkflow(JObject requestJsonObject)
        {
            DiagnosticActivity diagnosticActivity = null;
            Int32 workflowVersionId = -1;
            String commentsIfAny = String.Empty;
            String status = "Success";
            JObject responseJsonObject = new JObject();

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.ResumeWorkflow");
                diagnosticActivity.Start();
            }

            try
            {
                #region Read input json
              
                Boolean returnRequest = ValueTypeHelper.BooleanTryParse(requestJsonObject["returnRequest"].ToString(), false);
                String entityGUID = requestJsonObject["dataObjects"][0]["id"].ToString();
                String workflowName = requestJsonObject["requestParams"]["workflowName"].ToString();
                String currentActivityName = requestJsonObject["requestParams"]["currentActivityName"].ToString();
                String action = requestJsonObject["requestParams"]["action"].ToString();
                CallerContext callerContext = requestJsonObject["requestParams"]["callerContext"].ConvertToObject<CallerContext>();

                JToken comments = requestJsonObject["requestParams"]["comments"];
                if (comments != null)
                {
                    commentsIfAny = comments.ToString();
                }

                #endregion Read input json

                OperationResult operationResult = this.ResumeWorkflow(entityGUID, workflowName, workflowVersionId, currentActivityName, action, commentsIfAny, callerContext);

                #region Return JSON output

                if (operationResult.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    status = "Failure";
                }

                if (returnRequest)
                {
                    responseJsonObject.Add(new JProperty("dataObjectOperationRequest", requestJsonObject));
                }

                responseJsonObject.Add(new JProperty("dataObjectOperationResponse", new JObject(new JProperty("status", status))));

                #endregion Return JSON output
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

            return responseJsonObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JObject ChangeAssignment(JObject requestJsonObject)
        {
            JObject responseJsonObject = new JObject();
            DiagnosticActivity diagnosticActivity = null;

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.ChangeAssignment");
                diagnosticActivity.Start();
            }

            try
            {
                #region Read input json

                Boolean returnRequest = ValueTypeHelper.BooleanTryParse(requestJsonObject["returnRequest"].ToString(), false);
                String entityGUID = requestJsonObject["dataObjects"][0]["id"].ToString();
                String workflowName = requestJsonObject["requestParams"]["workflowName"].ToString();
                String currentActivityName = requestJsonObject["requestParams"]["currentActivityName"].ToString();
                String newlyAssignedUserName = requestJsonObject["requestParams"]["newlyAssignedUserName"].ToString();
                CallerContext callerContext = requestJsonObject["requestParams"]["callerContext"].ConvertToObject<CallerContext>();

                #endregion Read input json

                EntityOperationResult operationResult = this.ChangeAssignment(entityGUID, workflowName, currentActivityName, newlyAssignedUserName, callerContext);

                #region Return JSON output

                if (returnRequest)
                {
                    responseJsonObject.Add(new JProperty("dataObjectOperationRequest", requestJsonObject));
                }

                responseJsonObject.Add(new JProperty("dataObjectOperationResponse", new JObject(new JProperty("status", "Success"))));

                #endregion Return JSON output
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

            return responseJsonObject;
        }

        #region Helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="callerContext"></param>
        /// <param name="comments"></param>
        public OperationResult StartWorkflow(String entityGUID, String workflowName, Int32 workflowVersionId, CallerContext callerContext, String comments = "")
        {
            #region Initialization

            OperationResult result = null;
            DiagnosticActivity diagnosticActivity = null;
            WorkflowRuntimeBL workflowRuntimeBL = new WorkflowRuntimeBL();
            String loggedInUser = String.Empty;
            WorkflowInstanceRunOptions instanceRunOption = WorkflowInstanceRunOptions.RunAsMultipleInstances;
            EntityOperationResult operationResult = new EntityOperationResult() { EntityGUID = entityGUID };
            EntityOperationResultCollection operationResults = new EntityOperationResultCollection();
            operationResults.Add(operationResult);

            #endregion Initialization

            #region Diagnostics & Tracing

            if (_currentTraceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity = MDMTraceHelper.CreateTraceActivity(MDMTraceSource.AdvancedWorkflow, "EntityWorkflowBL.StartWorkflow5");
                diagnosticActivity.Start();
            }

            #endregion Diagnostics & Tracing

            try
            {
                #region Validation

                if (String.IsNullOrWhiteSpace(entityGUID))
                {
                    // TODO : Create locale message
                    throw new MDMOperationException("-99999", String.Empty, "EntityWorkflowBL", String.Empty, "StartWorkflowUsingEntityGUID");
                }

                #endregion Validation

                #region Prepare Workflow Data Context

                MDMBOW.WorkflowDataContext workflowDataContext = new MDMBOW.WorkflowDataContext();
                workflowDataContext.MDMObjectCollection = this.GetWorkflowMDMObjectCollectionUsingGUID(entityGUID);
                workflowDataContext.Module = callerContext.Module;
                workflowDataContext.Application = callerContext.Application;
                workflowDataContext.WorkflowName = workflowName;
                workflowDataContext.WorkflowVersionId = workflowVersionId;
                workflowDataContext.WorkflowComments = comments;

                #endregion Prepare Workflow Data Context

                #region Call Start Workflow

                result = workflowRuntimeBL.StartWorkflow(workflowDataContext, String.Empty, 0, loggedInUser, instanceRunOption, operationResults, callerContext);
                Int32 invokeSuccessCount = result.ReturnValues.Count > 0 ? ValueTypeHelper.Int32TryParse(result.ReturnValues.FirstOrDefault().ToString(), 0) : 0;

                UpdateEntityOperationResultCollection(operationResults, result, invokeSuccessCount, WorkflowAction_Start, callerContext);

                #endregion Call Start Workflow
            }
            finally
            {
                #region Diagnostics & Tracing

                if (_currentTraceSettings.IsBasicTracingEnabled)
                {
                    if (diagnosticActivity != null)
                    {
                        diagnosticActivity.Stop();
                    }
                }

                #endregion Diagnostics & Tracing
            }

            return operationResults.FirstOrDefault();
        }

        #endregion Helper methods
        
        #endregion Rest APIs

        #endregion Start Workflow

        #region Resume Workflow

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="action"></param>
        /// <param name="comments"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityOperationResultCollection ResumeWorkflow(Collection<Int64> entityIdList, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResults = null;

            #region Validation

            if (entityIdList == null || entityIdList.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111847", false, callerContext);
                throw new MDMOperationException("111847", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow"); //EntityIdList cannot be null or empty.
            }

            #endregion Validation

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : EntityIds to invoke workflow for : " + ValueTypeHelper.JoinCollection(entityIdList, ","));
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : WorkflowName : " + workflowName);
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : WorkflowVersionId : " + workflowVersionId);
            }

            EntityCollection entities = GetEntities(entityIdList, callerContext);

            if (entities != null)
            {
                entityOperationResults = new EntityOperationResultCollection();
                Int32 batchSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Entity.WorkflowTransition.BatchSize", 100);

                //If BatchSize is 0, set it to 100.
                if (batchSize == 0)
                {
                    batchSize = 100;
                }

                Int32 startIndex = 0;

                //Start Batching
                while (startIndex < entities.Count)
                {
                    Int32 endIndex = (startIndex + batchSize) - 1;

                    if (endIndex >= entities.Count)
                        endIndex = entities.Count - 1;

                    EntityCollection batchEntitiesIdList = new EntityCollection();

                    //Take entities between startIndex and endIndex from entity identifier list
                    for (Int32 i = startIndex; i <= endIndex; i++)
                    {
                        Entity batchEntity = entities.ElementAt(i);

                        if (batchEntity != null)
                        {
                            batchEntitiesIdList.Add(batchEntity);
                        }
                    }

                    EntityOperationResultCollection batchEntityOperationResult = this.ResumeWorkflow(batchEntitiesIdList, workflowName, workflowVersionId, currentActivityName, action, comments, callerContext);

                    UpdateMasterEnitityOperationResult(entityOperationResults, batchEntityOperationResult);

                    startIndex = startIndex + batchSize;
                }
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities found to resume workflow", MDMTraceSource.AdvancedWorkflow);
            }

            return entityOperationResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="action"></param>
        /// <param name="comments"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityOperationResultCollection ResumeWorkflow(EntityCollection entities, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext)
        {
            #region Validation

            if (entities == null || entities.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
                throw new MDMOperationException("111816", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "StartWorkflow");//Entities collection is null or empty
            }

            #endregion Validation

            //Initialize EntityOperationResultCollection before calling any BR.
            EntityOperationResultCollection entityOperationResults = Utility.PrepareEntityOperationResultsSchema(entities);

            try
            {
                #region Initialize objects

                String userName = String.Empty;
                Int32 userId = -1;

                if (_securityPrincipal != null)
                {
                    userName = _securityPrincipal.CurrentUserName;
                    userId = _securityPrincipal.CurrentUserId;
                }

                //Initialize WorkflowActionContext
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = userName;
                actionContext.ActingUserId = userId;

                //Set WorkflowActionContext for each entities which came for process. this is required to pass workflow action information to BR.
                foreach (Entity entity in entities)
                {
                    IWorkflowStateCollection iwfStates = entity.GetWorkflowDetails();

                    if (iwfStates != null)
                    {
                        foreach (IWorkflowState wfState in iwfStates)
                        {
                            if (wfState.ActivityShortName == currentActivityName)
                            {
                                actionContext.CurrentActivityLongName = wfState.ActivityLongName;
                                actionContext.WorkflowName = wfState.WorkflowName;
                                break;
                            }
                        }
                    }

                    entity.WorkflowActionContext = actionContext;
                }

                #endregion Initialize objects

                #region Fire Pre event

                //Validate StateViews Before Firing the Pre Transition Event.
                if (!ValidateStateViews(entities, entityOperationResults, callerContext, actionContext.WorkflowName, actionContext.CurrentActivityLongName, action))
                {
                    //No entities qualified for further processing. So return entity operation result and exit.
                    return entityOperationResults;
                }

                var continueProcess = EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(entities, entityOperationResults, callerContext, EventSource.EntityTransitioning, _entityManager, _securityPrincipal.CurrentUserId);

                if (!continueProcess)
                {
                    return entityOperationResults;
                }

                #endregion Fire Pre event

                #region CheckforExitCriteria

                foreach (Entity entity in entities)
                {
                    var entityOperationResult = entityOperationResults.GetEntityOperationResult(entity.Id);
                    BusinessRuleProcessor.EvaluateActivityExitCriteria(entity, entity.WorkflowActionContext, workflowVersionId, currentActivityName, callerContext, entityOperationResult);
                }

                entityOperationResults.RefreshOperationResultStatus();
                var sourceEntities = new EntityCollection(entities.ToXml());    // Since scan and filter method removing the entities required the source entities.

                EntityWorkflowOperationsCommonUtility.ScanAndFilterEntities(entities, entityOperationResults);

                #endregion

                #region Resume Workflow

                if (entities.Any())
                {
                    //Create WorkflowDataContext
                    var workflowDataContext = new MDMBOW.WorkflowDataContext { WorkflowName = workflowName, WorkflowVersionId = workflowVersionId, MDMObjectCollection = this.GetWorkflowMDMObjectCollection(entities) };

                    //Resume workflow
                    var operationResult = new OperationResult();
                    var workflowRuntimeManager = new WorkflowRuntimeBL();
                    Int32 resumeSuccessCount = workflowRuntimeManager.ResumeWorkflow(workflowDataContext, actionContext, ref operationResult, callerContext);

                    UpdateEntityOperationResultCollection(entityOperationResults, operationResult, resumeSuccessCount, WorkflowAction_Resume, callerContext);
                }

                #endregion Resume Workflow

                #region Process Entity Validation States

                //Process the Entity Validation States.
                EntityStateValidationBL entityStateValidationBL = new EntityStateValidationBL();
                entityStateValidationBL.Process(sourceEntities, entityOperationResults, callerContext);

                #endregion Process Entity Validation States

                #region Fire Post event

                //Resume Workflow is ASync functionality and hence this is not the appropriate position to fire Post Events. Hence commenting.
                //This has been moved to the ActivityTrackingBL, and being fired on 'Executing' record of the next activity as the transition will be 
                //successfully completed by that time.

                ////Fire Post event.
                //if (!EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(entities, entityOperationResults, callerContext, EventSource.EntityTransitioned, _entityManager, _securityPrincipal.CurrentUserId))
                //{
                //    //No entities qualified for further processing. So return entity operation result and exit.
                //    return entityOperationResults;
                //}

                #endregion Fire Post event
            }
            catch (Exception ex)
            {
                const string exceptionMessageFormat = "Workflow action failed for entities names: [{0}]. Internal Error is: {1}";
                String exceptionMessage = String.Format(exceptionMessageFormat, entities.GetEntityNamesString(), ex.Message);

                var appException = new ApplicationException(exceptionMessage, ex);

                new ExceptionHandler(appException);

                var exceptionOperationResult = new OperationResult();
                exceptionOperationResult.AddOperationResult(String.Empty, appException.Message, OperationResultType.Error);

                //Update this exception to each entity in the transaction
                UpdateEntityOperationResultCollection(entityOperationResults, exceptionOperationResult, 0, WorkflowAction_Resume, callerContext);
            }
            finally
            {
            }

            return entityOperationResults;
        }
    
        #endregion Resume Workflow

        #region Change Assignment

        /// <summary>
        /// Change the workflow activity's ownership assignment from one user to another user.
        /// </summary>
        /// <param name="entityIds">Indicates the list of Entity Ids</param>
        /// <param name="currentActivityName">Indicates the current activity name</param>
        /// <param name="newlyAssignedUser">Indicates the Newly assigned security user</param>
        /// <param name="assignmentAction">Indicates the Assignment Action</param>
        /// <param name="callerContext">Indicates the Caller context details</param>
        /// <returns>EntityOperationResultCollection</returns>
        public EntityOperationResultCollection ChangeAssignment(Collection<Int64> entityIds, String currentActivityName, SecurityUser newlyAssignedUser, String assignmentAction, CallerContext callerContext)
        {
            //TODO:: If Different Entities has different workflow's then how to handle
            //TODO:: How to support multiple activities change assignments.
            EntityOperationResultCollection entityORC = new EntityOperationResultCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow, false);

            #region Validations

            if (entityIds == null || entityIds.Count < 1)
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111847", false, callerContext);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                throw new MDMOperationException("111847", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");    //EntityIdList cannot be null or empty.
            }

            if (String.IsNullOrWhiteSpace(currentActivityName))
            {
                _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112010", false, callerContext);

                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                throw new MDMOperationException("112010", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");  //Activity Name cannot be null or empty.
            }

            #endregion Validations

            EntityCollection entities = GetEntities(entityIds, callerContext);

            if (entities != null && entities.Count > 0)
            {
                Int32 batchSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Entity.WorkflowTransition.BatchSize", 100);

                //If BatchSize is 0, set it to 100.
                if (batchSize == 0)
                {
                    batchSize = 100;
                }

                Int32 startIndex = 0;

                //Start Batching
                while (startIndex < entities.Count)
                {
                    Int32 endIndex = (startIndex + batchSize) - 1;

                    if (endIndex >= entities.Count)
                        endIndex = entities.Count - 1;

                    EntityCollection batchEntities = new EntityCollection();

                    //Take entities between startIndex and endIndex from entity identifier list
                    for (Int32 i = startIndex; i <= endIndex; i++)
                    {
                        Entity batchEntity = entities.ElementAt(i);

                        if (batchEntity != null)
                        {
                            batchEntities.Add(batchEntity);
                        }
                    }

                    EntityOperationResultCollection batchEntityOperationResult = this.ChangeAssignment(batchEntities, currentActivityName, newlyAssignedUser, assignmentAction, callerContext);
                    UpdateMasterEnitityOperationResult(entityORC, batchEntityOperationResult);

                    startIndex = startIndex + batchSize;
                }
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities found to change the workflow activity", MDMTraceSource.AdvancedWorkflow);
            }

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StopTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow);

            return entityORC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="newlyAssignedUser"></param>
        /// <param name="assignmentAction"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityOperationResultCollection ChangeAssignment(EntityCollection entities, String currentActivityName, SecurityUser newlyAssignedUser, String assignmentAction, CallerContext callerContext)
        {
            Boolean isRelease = false;

            if (assignmentAction == "Release")
            {
                assignmentAction = "";
                isRelease = true;
            }

            //TODO:: If Different Entities has different workflow's then how to handle
            //TODO:: How to support multiple activities change assignments.
            EntityOperationResultCollection entityORC = new EntityOperationResultCollection();

            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.StartTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow, false);

            try
            {
                String newlyAssignedUserName = String.Empty;
                Int32 newlyAssignedUserId = 0;

                #region Validations

                if (entities == null || entities.Count < 1)
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111847", false, callerContext);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                    throw new MDMOperationException("111847", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");    //EntityIdList cannot be null or empty.
                }

                if (String.IsNullOrWhiteSpace(currentActivityName))
                {
                    _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112010", false, callerContext);

                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                    throw new MDMOperationException("112010", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");  //Activity Name cannot be null or empty.
                }

                if (newlyAssignedUser != null)
                {
                    newlyAssignedUserName = newlyAssignedUser.UserName;
                    newlyAssignedUserId = newlyAssignedUser.Id;
                }

                if (String.IsNullOrWhiteSpace(newlyAssignedUserName))
                {
                    if (newlyAssignedUserId > 0)
                    {
                        Collection<Int32> mdmUserId = new Collection<Int32>();
                        mdmUserId.Add(newlyAssignedUserId);

                        SecurityUserBL securityUserBL = new AdminManager.Business.SecurityUserBL();
                        SecurityUserCollection SecurityUserCollection = securityUserBL.GetUsersByIds(mdmUserId);

                        if (SecurityUserCollection != null && SecurityUserCollection.Count > 0)
                        {
                            newlyAssignedUser = SecurityUserCollection.FirstOrDefault();
                            newlyAssignedUserName = newlyAssignedUser.UserName;
                        }
                    }

                    //By pass validation if assignment Action is ReleaseOwnership.
                    if (String.IsNullOrWhiteSpace(newlyAssignedUserName) && !isRelease)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111998", false, callerContext);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                        throw new MDMOperationException("111998", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");    // Newly assigned username is null or empty
                    }
                }

                #endregion

                String entityIdList = ValueTypeHelper.JoinCollection(entities.GetEntityIdList(), ",");

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : EntityIds are : " + entityIdList, MDMTraceSource.AdvancedWorkflow);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : currentActivityName : " + currentActivityName, MDMTraceSource.AdvancedWorkflow);
                }

                if (entities != null && entities.Count > 0)
                {
                    #region Initialize objects

                    //Initialize EntityOperationResultCollection before calling any BR.
                    entityORC = Utility.PrepareEntityOperationResultsSchema(entities);

                    String userName = String.Empty;
                    Int32 userId = -1;
                    IWorkflowState iwfState = null;

                    if (_securityPrincipal != null)
                    {
                        userName = _securityPrincipal.CurrentUserName;
                        userId = _securityPrincipal.CurrentUserId;
                    }

                    //Get the workflow details... Assumed 
                    IWorkflowStateCollection iwfStates = entities.FirstOrDefault().GetWorkflowDetails();

                    foreach (IWorkflowState wfState in iwfStates)
                    {
                        if (wfState.ActivityShortName == currentActivityName)
                        {
                            iwfState = wfState;
                            break;
                        }
                    }

                    if (iwfState == null)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112009", false, callerContext);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                        throw new MDMOperationException("112009", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment");    // Change Assignment failed. Requested entities are not currently active for provided activity.
                    }

                    #region Prepare WorkflowActionContext

                    //Initialize WorkflowActionContext
                    MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                    actionContext.UserAction = assignmentAction;
                    actionContext.CurrentActivityName = currentActivityName;
                    actionContext.ActingUserName = userName;
                    actionContext.ActingUserId = userId;
                    actionContext.NewlyAssignedUserName = newlyAssignedUserName;
                    actionContext.NewlyAssignedUserId = newlyAssignedUserId;
                    actionContext.PreviousAssignedUserName = iwfState.AssignedUser;
                    actionContext.PreviousAssignedUserId = iwfState.AssignedUserId;
                    actionContext.WorkflowName = iwfState.WorkflowName;
                    actionContext.WorkflowLongName = iwfState.WorkflowLongName;
                    actionContext.CurrentActivityLongName = iwfState.ActivityLongName;

                    #endregion Prepare WorkflowActionContext

                    //Set WorkflowActionContext for each entities which came for process. this is required to pass workflow action information to BR.
                    foreach (Entity entity in entities)
                    {
                        entity.WorkflowActionContext = actionContext;
                    }

                    #endregion Initialize objects

                    #region Fire Pre event

                    //Fire Entity Assignment Changing Event.
                    if (!EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(entities, entityORC, callerContext, EventSource.EntityAssignmentChanging, _entityManager, _securityPrincipal.CurrentUserId))
                    {
                        //No entities qualified for further processing. So return operation result and exit.
                        return entityORC;
                    }

                    #endregion Fire Pre event

                    #region Assignment Workflow

                    //Resume workflow
                    WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL(true);

                    String user = newlyAssignedUserName;
                    if (isRelease)
                    {
                        user = "";
                    }

                    iwfState.PreviousActivityUser = iwfState.AssignedUser;
                    iwfState.AssignedUser = user;

                    Boolean isUpdated = workflowInstanceBL.UpdateWorkflowInstances(String.Empty, entityIdList, "MDM.BusinessObjects.Entity", iwfState.WorkflowId, currentActivityName, assignmentAction, user, callerContext);

                    if (!isUpdated)
                    {
                        _localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112013", false, callerContext);

                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, _localeMessage.Message, MDMTraceSource.AdvancedWorkflow);

                        throw new MDMOperationException("112013", _localeMessage.Message, "EntityWorkflowBL", String.Empty, "ChangeAssignment"); //Failed to change assignment. Please check the provided activity details and user details.
                    }

                    #region Send assignment change event to jigsaw

                    if (JigsawConstants.IsJigsawIntegrationEnabled)
                    {
                        foreach (Entity entity in entities)
                        {
                            SendWorkflowAssignmentEventToJigsaw(entity, iwfState, callerContext, actionContext.ActingUserName, actionContext.UserAction);
                        }
                    }

                    #endregion

                    #endregion Assignment Workflow

                    #region Fire Post event

                    //Fire Post event.
                    if (!EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(entities, entityORC, callerContext, EventSource.EntityAssignmentChanged, _entityManager, _securityPrincipal.CurrentUserId))
                    {
                        //No entities qualified for further processing. So return  operation result and exit.
                        return entityORC;
                    }

                    #endregion Fire Post event

                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities found to change the workflow activity", MDMTraceSource.AdvancedWorkflow);
                }
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityORC.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityORC.AddOperationResult("111995", ex.Message, OperationResultType.Error);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow);
            }

            return entityORC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="currentWorkflowState"></param>
        /// <param name="callerContext"></param>
        /// <param name="actingUser"></param>
        private static void SendWorkflowAssignmentEventToJigsaw(Entity entity, IWorkflowState currentWorkflowState, CallerContext callerContext, String actingUser, String performedAction)
        {
            var eventDataPackage = new EventDataPackage
            {
                EventType = EventType.workflowAssignmentChanged,
                EventSubType = EventSubType.none,
                ActingUser = actingUser,
                SourceEntity = entity,
                TimeStamp = DateTime.Now,
                EventSourceName = String.Empty,
                EventGroupId = null,
            };

            if (currentWorkflowState != null)
            {
                var workflowEventData = new WorkflowEventData
                {
                    WorkflowName = currentWorkflowState.WorkflowName,
                    WorkflowVersion = currentWorkflowState.WorkflowVersionName,

                    WorkflowStageFrom = currentWorkflowState.PreviousActivityLongName,
                    WorkflowStageTo = currentWorkflowState.ActivityLongName,

                    WorkflowAssignedFrom = currentWorkflowState.PreviousActivityUser,
                    WorkflowAssignedTo = currentWorkflowState.AssignedUser,

                    WorkflowStageActionTaken = String.IsNullOrWhiteSpace(performedAction) ? currentWorkflowState.PreviousActivityAction : performedAction,

                    WorkflowStartTime = DateTime.Now,
                    WorkflowEndTime = DateTime.Now,

                    WorkflowStageFromDateTime = ValueTypeHelper.DateTimeTryParse(currentWorkflowState.PreviousActivityEventDate, DateTime.Now),
                    WorkflowStageToDateTime = ValueTypeHelper.DateTimeTryParse(currentWorkflowState.EventDate, DateTime.Now)
                };

                eventDataPackage.EventData = workflowEventData;
            }

            MessageBrokerHelper.SendEventMessage(new List<EventDataPackage> { eventDataPackage }, callerContext, JigsawCallerProcessType.WorkflowEvent, JigsawConstants.IntegrationBrokerType, actingUser);
        }
     
        #endregion Change Assignment

        /// <summary>
        /// Performs Bulk Workflow Action in batch
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="activityLongName"></param>
        /// <param name="workflowAction"></param>
        /// <param name="comments"></param>
        /// <param name="operationType"></param>
        /// <param name="newlyAssignedUser"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public EntityOperationResultCollection PerformBulkWorkflowOperation(Collection<Int64> entityIdList, String activityLongName, String workflowAction, String comments, String operationType, SecurityUser newlyAssignedUser, CallerContext callerContext)
        {

            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection();

            Dictionary<String, EntityCollection> map = new Dictionary<String, EntityCollection>();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityWorkflowBL.PerformBulkWorkflowOperation", MDMTraceSource.AdvancedWorkflow, false);
            }

            try
            {
                if (!entityIdList.IsNullOrEmpty())
                {
                    #region CombineActivityShortNameWithEntities

                    EntityCollection entities = GetEntities(entityIdList, callerContext);
                    if (!entities.IsNullOrEmpty())
                    {
                        foreach (Entity entity in entities)
                        {
                            IWorkflowStateCollection workflowStates = entity.GetWorkflowDetails();
                            if (!workflowStates.IsNullOrEmpty())
                            {
                                foreach (IWorkflowState workflowState in workflowStates.Where(x => x.ActivityLongName.Equals(activityLongName, StringComparison.OrdinalIgnoreCase)))
                                {
                                    String activityShortName = workflowState.ActivityShortName;
                                    if (!map.ContainsKey(activityShortName))
                                    {
                                        map.Add(activityShortName, new EntityCollection());
                                    }
                                    map[activityShortName].Add(entity);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Constants.TRACING_ENABLED)
                            MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities found for the given entity IDs", MDMTraceSource.AdvancedWorkflow);
                    }

                    #endregion CombineActivityShortNameWithEntities

                    List<EntityOperationResultCollection> operationsResults = new List<EntityOperationResultCollection>(map.Count);
                    foreach (KeyValuePair<String, EntityCollection> shortNameWithEntities in map)
                    {
                        EntityCollection entityCollection = new EntityCollection(shortNameWithEntities.Value.ToList());
                        String activityShortName = shortNameWithEntities.Key;
                        EntityOperationResultCollection operationResult = PerformBatchWorkflowAction(entityCollection, activityShortName, workflowAction, comments, operationType, newlyAssignedUser, callerContext, "", 0);
                        operationsResults.Add(operationResult);
                    }

                    entityOperationResultCollection = MergeEntityOperationResultCollections(operationsResults, callerContext);
                }
                else
                {
                    if (Constants.TRACING_ENABLED)
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "No entities found to change the workflow activity", MDMTraceSource.AdvancedWorkflow);
                }
            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityOperationResultCollection.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityOperationResultCollection.AddOperationResult("", ex.Message, OperationResultType.Error);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityWorkflowBL.PerformBulkWorkflowOperation", MDMTraceSource.AdvancedWorkflow);
            }

            return entityOperationResultCollection;
        }

        #region Private Methods
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersionId"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="action"></param>
        /// <param name="comments"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityOperationResult ResumeWorkflow(String entityGUID, String workflowName, Int32 workflowVersionId, String currentActivityName, String action, String comments, CallerContext callerContext)
        {
            OperationResult operationResult = new OperationResult();
            WorkflowRuntimeBL workflowRuntimeManager = new WorkflowRuntimeBL();
            EntityOperationResult entityOperationResult = new EntityOperationResult() { EntityGUID = entityGUID };

            #region Validation

            if (String.IsNullOrWhiteSpace(entityGUID))
            {
                // TODO : replace with message code
                //_localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "111816", false, callerContext);
                throw new MDMOperationException("111816", "", "EntityWorkflowBL", String.Empty, "ResumeWorkflow");//Entities collection is null or empty
            }

            #endregion Validation

            try
            {
                #region Initialize objects

                String userName = String.Empty;
                Int32 userId = -1;

                //if (_securityPrincipal != null)
                //{
                //    userName = _securityPrincipal.CurrentUserName;
                //    userId = _securityPrincipal.CurrentUserId;
                //}

                //Initialize WorkflowActionContext
                MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                actionContext.UserAction = action;
                actionContext.Comments = comments;
                actionContext.CurrentActivityName = currentActivityName;
                actionContext.ActingUserName = userName;
                actionContext.ActingUserId = userId;

                WorkflowInstanceBL workflowInstanceBL = new WorkflowInstanceBL();
                IWorkflowStateCollection iwfStates = workflowInstanceBL.LoadWorkflowDetails(entityGUID, workflowName, callerContext);

                if (iwfStates != null)
                {
                    foreach (IWorkflowState wfState in iwfStates)
                    {
                        if (wfState.ActivityShortName == currentActivityName)
                        {
                            actionContext.CurrentActivityLongName = wfState.ActivityLongName;
                            actionContext.WorkflowName = wfState.WorkflowName;
                            break;
                        }
                    }
                }

                #endregion Initialize objects

                #region Fire Pre event

                #endregion Fire Pre event

                #region Resume Workflow

                var workflowDataContext = new MDMBOW.WorkflowDataContext { WorkflowName = workflowName, WorkflowVersionId = workflowVersionId, MDMObjectCollection = this.GetWorkflowMDMObjectCollectionUsingGUID(entityGUID) };
               
                Int32 resumeSuccessCount = workflowRuntimeManager.ResumeWorkflow(workflowDataContext, actionContext, ref operationResult, callerContext);

                UpdateEntityOperationResult(entityOperationResult, operationResult, resumeSuccessCount, WorkflowAction_Resume, callerContext);

                #endregion Resume Workflow

                #region Fire Post event

                #endregion Fire Post event

            }
            catch (Exception ex)
            {
                const string exceptionMessageFormat = "Workflow action failed. Internal Error is: {1}";
                String exceptionMessage = String.Format(exceptionMessageFormat, ex.Message);

                // TODO: handle exception code
                //var appException = new ApplicationException(exceptionMessage);

                //new ExceptionHandler(appException);

                //var exceptionOperationResult = new OperationResult();
                //exceptionOperationResult.AddOperationResult(String.Empty, ex.Message, OperationResultType.Error);

                ////Update this exception to each entity in the transaction
                //UpdateEntityOperationResultCollection(entityOperationResults, exceptionOperationResult, 0, WorkflowAction_Resume, callerContext);

            }

            return entityOperationResult;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityGUID"></param>
        /// <param name="workflowName"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="newlyAssignedUserName"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityOperationResult ChangeAssignment(String entityGUID, String workflowName, String currentActivityName, String newlyAssignedUserName, CallerContext callerContext)
        {
            IWorkflowState iwfState = null;
            String user = String.Empty;
            String instanceStatus = String.Empty;
            EntityOperationResult entityOR = new EntityOperationResult();
            WorkflowInstanceBL _workflowInstanceBL = new WorkflowInstanceBL();

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.StartTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow, false);
            }

            try
            {
                #region Validations

                if (String.IsNullOrWhiteSpace(entityGUID))
                {

                }

                if (String.IsNullOrWhiteSpace(currentActivityName))
                {

                }

                if (String.IsNullOrWhiteSpace(newlyAssignedUserName))
                {
                   
                }

                #endregion Validations

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : EntityGUID is : " + entityGUID, MDMTraceSource.AdvancedWorkflow);
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Param : currentActivityName : " + currentActivityName, MDMTraceSource.AdvancedWorkflow);
                }

                #region Initialize objects

                IWorkflowStateCollection iwfStates = _workflowInstanceBL.LoadWorkflowDetails(entityGUID, workflowName, callerContext);

                foreach (IWorkflowState wfState in iwfStates)
                {
                    if (wfState.ActivityShortName == currentActivityName)
                    {
                        iwfState = wfState;
                        break;
                    }
                }

                if (iwfState == null)
                {
                    throw new MDMOperationException("112009", "", "EntityWorkflowBL", String.Empty, "ChangeAssignment");    // Change Assignment failed. Requested entities are not currently active for provided activity.
                }

                #region Prepare WorkflowActionContext

                //Initialize WorkflowActionContext
                //MDMBOW.WorkflowActionContext actionContext = new MDMBOW.WorkflowActionContext();
                //actionContext.UserAction = assignmentAction;
                //actionContext.CurrentActivityName = currentActivityName;
                //actionContext.ActingUserName = userName;
                //actionContext.ActingUserId = userId;
                //actionContext.NewlyAssignedUserName = newlyAssignedUserName;
                //actionContext.NewlyAssignedUserId = newlyAssignedUserId;
                //actionContext.PreviousAssignedUserName = iwfState.AssignedUser;
                //actionContext.PreviousAssignedUserId = iwfState.AssignedUserId;
                //actionContext.WorkflowName = iwfState.WorkflowName;
                //actionContext.WorkflowLongName = iwfState.WorkflowLongName;
                //actionContext.CurrentActivityLongName = iwfState.ActivityLongName;

                #endregion Prepare WorkflowActionContext

                #endregion Initialize objects

                #region Fire Pre event

                #endregion Fire Pre event

                #region Assignment Workflow

                // Validating newlyAssignedUserName to check if any user exists in DB with this name or not
                SecurityUserBL securityUserBL = new SecurityUserBL();
                SecurityUser securityUser = securityUserBL.GetUser(newlyAssignedUserName);
                if (securityUser != null)
                {
                    user = newlyAssignedUserName;
                }

                iwfState.PreviousActivityUser = iwfState.AssignedUser;
                iwfState.AssignedUser = user;

                Boolean isUpdated = _workflowInstanceBL.UpdateWorkflowInstances(String.Empty, entityGUID, "MDM.BusinessObjects.Entity", iwfState.WorkflowId, currentActivityName, instanceStatus, user, callerContext);

                if (!isUpdated)
                {
                    entityOR.OperationResultStatus = OperationResultStatusEnum.Failed;
                    throw new MDMOperationException("112013", "", "EntityWorkflowBL", String.Empty, "ChangeAssignment"); //Failed to change assignment. Please check the provided activity details and user details.
                }

                #region Send assignment change event to jigsaw

                //if (JigsawConstants.IsJigsawIntegrationEnabled)
                //{
                //    foreach (Entity entity in entities)
                //    {
                //        SendWorkflowAssignmentEventToJigsaw(entity, iwfState, callerContext, actionContext.ActingUserName, actionContext.UserAction);
                //    }
                //}

                #endregion

                #endregion Assignment Workflow

                #region Fire Post event

                //Fire Post event.
                //if (!EntityWorkflowOperationsCommonUtility.FireEntityWorkflowEvent(entities, entityORC, callerContext, EventSource.EntityAssignmentChanged, _entityManager, _securityPrincipal.CurrentUserId))
                //{
                //    //No entities qualified for further processing. So return  operation result and exit.
                //    return entityORC;
                //}

                #endregion Fire Post event

            }
            catch (MDMOperationException ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityOR.AddOperationResult(ex.MessageCode, ex.Message, OperationResultType.Error);
            }
            catch (Exception ex)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Error, ex.Message, MDMTraceSource.General);
                entityOR.AddOperationResult("111995", ex.Message, OperationResultType.Error);
            }
            finally
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.StopTraceActivity("EntityWorkflowBL.ChangeAssignment", MDMTraceSource.AdvancedWorkflow);
            }

            return entityOR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private MDMBOW.WorkflowMDMObjectCollection GetWorkflowMDMObjectCollection(EntityCollection entities)
        {
            MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
            String mdmObjectType = "MDM.BusinessObjects.Entity";

            foreach (Entity entity in entities)
            {
                mdmObjectCollection.Add(new MDMBOW.WorkflowMDMObject(entity.Id, mdmObjectType));
            }

            return mdmObjectCollection;
        }

        /// <summary>
        ///  #Dhruma 
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        private MDMBOW.WorkflowMDMObjectCollection GetWorkflowMDMObjectCollectionUsingGUID(String mdmObjectGUID)
        {
            MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();
            String mdmObjectType = "MDM.BusinessObjects.Entity";

            MDMBOW.WorkflowMDMObject mdmObject = new MDMBOW.WorkflowMDMObject() { MDMObjectGUID = mdmObjectGUID, MDMObjectType = mdmObjectType };
            mdmObjectCollection.Add(mdmObject);

            return mdmObjectCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private MDMBOW.WorkflowMDMObjectCollection GetWorkflowMDMObjectCollection(Entity entity)
        {
            MDMBOW.WorkflowMDMObjectCollection mdmObjectCollection = new MDMBOW.WorkflowMDMObjectCollection();

            String mdmObjectType = "MDM.BusinessObjects.Entity";
            mdmObjectCollection.Add(new MDMBOW.WorkflowMDMObject(entity.Id, mdmObjectType));

            return mdmObjectCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityOperationResult"></param>
        /// <param name="operationResult"></param>
        /// <param name="successCount"></param>
        /// <param name="workflowAction"></param>
        /// <param name="callerContext"></param>
        private void UpdateEntityOperationResult(EntityOperationResult entityOperationResult, OperationResult operationResult, Int32 successCount, String workflowAction, CallerContext callerContext)
        {
            if (operationResult != null && entityOperationResult != null)
            {
                entityOperationResult.OperationResultStatus = operationResult.OperationResultStatus;

                // If invoked sucessfully log sucess info.
                if (entityOperationResult.OperationResultStatus == OperationResultStatusEnum.Successful)
                {
                    // RBC - need to change message. Message is not present.
                    #region Add info for successful operation
                    String messageCode = String.Empty;

                    //Set information for no. of successful entity for start / resume workflow.
                    if (workflowAction == WorkflowAction_Start)
                    {
                        //Message = Workflow invoked successfully for {0} entities.
                        messageCode = "111948";
                    }
                    else
                    {
                        //Message = Workflow resumed successfully for {0} entities.
                        messageCode = "111949";
                    }

                    //_localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, new Object[] { successCount }, false, callerContext);
                    entityOperationResult.Informations.Add(new Information(messageCode, "", new Collection<Object>() { successCount }));
                    #endregion
                }

                //Log errors
                if (operationResult.Errors != null)
                {
                    foreach (Error error in operationResult.Errors)
                    {
                        entityOperationResult.Errors.Add(error);
                    }
                }

                //Log information
                if (operationResult.Informations != null)
                {
                    foreach (Information info in operationResult.Informations)
                    {
                        entityOperationResult.Informations.Add(info);
                    }
                }

                //Log warning
                if (operationResult.Warnings != null)
                {
                    foreach (Warning warning in operationResult.Warnings)
                    {
                        entityOperationResult.Warnings.Add(warning);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityORC"></param>
        /// <param name="operationResult"></param>
        /// <param name="successCount"></param>
        /// <param name="workflowAction"></param>
        /// <param name="callerContext"></param>
        private void UpdateEntityOperationResultCollection(EntityOperationResultCollection entityORC, OperationResult operationResult, Int32 successCount, String workflowAction, CallerContext callerContext)
        {
            if (operationResult != null && entityORC != null)
            {
                String messageCode = String.Empty;
                if (!operationResult.HasError)
                {
                    //Set information for no. of successful entity for start / resume workflow.
                    if (workflowAction == WorkflowAction_Start)
                    {
                        //Message = Workflow invoked successfully for {0} entities.
                        messageCode = "111948";
                    }
                    else
                    {
                        //Message = Workflow resumed successfully for {0} entities.
                        messageCode = "111949";
                    }

                    //_localeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), messageCode, new Object[] { successCount }, false, callerContext);
                    entityORC.Informations.Add(new Information(messageCode, "", new Collection<Object>() { successCount }));
                }

                //Log errors
                if (operationResult.Errors != null)
                {
                    //Int64 entityId = 0;
                    //Error entityOperationResultError = null;
                    //Hashtable operationResultStatusTable = new Hashtable();

                    foreach (Error error in operationResult.Errors)
                    {
                        entityORC.Errors.Add(error);
                    }

                    //if (operationResult.ReturnValues != null)
                    //{
                    //    foreach (Object obj in operationResult.ReturnValues)
                    //    {
                    //        if (obj != null && obj is Hashtable)
                    //        {
                    //            operationResultStatusTable = (Hashtable)obj;
                    //            foreach (DictionaryEntry properties in operationResultStatusTable)
                    //            {
                    //                entityId = ValueTypeHelper.Int64TryParse(properties.Key.ToString(), 0);
                    //                entityOperationResultError = (Error)operationResultStatusTable[properties.Key];
                    //                if (entityOperationResultError != null)
                    //                {
                    //                    entityORC.AddEntityOperationResult(entityId, entityOperationResultError.ErrorCode, entityOperationResultError.ErrorMessage, OperationResultType.Error);
                    //                }
                    //            }
                    //        }

                    //    }
                    //}
                }

                //Log information
                if (operationResult.Informations != null)
                {
                    foreach (Information info in operationResult.Informations)
                    {
                        entityORC.Informations.Add(info);
                    }
                }

                //Log warning
                if (operationResult.Warnings != null)
                {
                    foreach (Warning warning in operationResult.Warnings)
                    {
                        entityORC.Warnings.Add(warning);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityIdList"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private EntityCollection GetEntities(Collection<Int64> entityIdList, CallerContext callerContext)
        {
            if (Constants.TRACING_ENABLED)
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to get entities from entityIdList", MDMTraceSource.AdvancedWorkflow);

            EntityCollection entities = null;

            if (entityIdList != null)
            {
                //Create entityContext
                EntityContext entityContext = new EntityContext();
                entityContext.LoadWorkflowInformation = true;
                entityContext.LoadEntityProperties = true;
                entityContext.DataLocales.Add(GlobalizationHelper.GetSystemDataLocale());

                //Set publish events and AVS as false, since this is an internal get
                entities = _entityManager.Get(entityIdList, entityContext, false, callerContext.Application, callerContext.Module, false, false, false);
            }
            else
            {
                if (Constants.TRACING_ENABLED)
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "EntityIdList is null", MDMTraceSource.AdvancedWorkflow);
            }

            return entities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventEntities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callContext"></param>
        /// <param name="eventSource"></param>
        /// <returns></returns>
        private Boolean ValidateStateViews(EntityCollection eventEntities, EntityOperationResultCollection entityOperationResults, CallerContext callContext, String workflowName, String currentActivityLongName, String workflowAction)
        {
            MDMPublisher publisher = Utility.GetMDMPublisher(callContext.Application, callContext.Module);
            if (callContext.MDMPublisher == MDMPublisher.Unknown)
            {
                callContext.MDMPublisher = publisher;
            }

            if (Constants.TRACING_ENABLED)
            {
                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "Trying to validate stateviews", MDMTraceSource.AdvancedWorkflow);
            }

            try
            {
                LookupBL lookupManager = new LookupBL();
                var stateviewWorkflowDepedancyLookupRows = lookupManager.GetLookupRows(STATEVIEW_WORKFLOW_DEPENDENCY, GlobalizationHelper.GetSystemDataLocale(), null, -1, callContext);

                if (!((stateviewWorkflowDepedancyLookupRows != null) && stateviewWorkflowDepedancyLookupRows.Count > 0))
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: No mappings defined in lookup StateView_Workflow_Dependecy.");
                    }
                    return true;
                }

                var stateviewLookupRows = lookupManager.GetLookupRows(STATEVIEW, GlobalizationHelper.GetSystemDataLocale(), null, -1, callContext);

                if (!((stateviewLookupRows != null) && stateviewLookupRows.Count > 0))
                {
                    if (Constants.TRACING_ENABLED)
                    {
                        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, "MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: No mappings defined in lookup StateViews.");
                    }
                    return true;
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: State View Rows {0}, Workflow Dependency Rows {1}", (stateviewLookupRows != null) ? stateviewLookupRows.Count() : 0, (stateviewWorkflowDepedancyLookupRows != null) ? stateviewWorkflowDepedancyLookupRows.Count() : 0), MDMTraceSource.AdvancedWorkflow);
                }

                //Filter to get only statesviews that need to be validated for the current workflow and activity
                RowCollection filteredStateviewWorkflowDepedancyRows = new RowCollection();

                foreach (Row row in stateviewWorkflowDepedancyLookupRows)
                {
                    var ifValidationRequired = row.GetValue("Validate");
                    Boolean validate = ifValidationRequired != null ? ValueTypeHelper.BooleanTryParse(ifValidationRequired.ToString(), false) : false;

                    if (validate)
                    {
                        var wfName = row.GetValue("WorkflowName");
                        var activityName = row.GetValue("WorkflowActivityName");
                        var workflowActionName = row.GetValue("WorkflowActionName");

                        if (wfName != null && String.Equals(wfName, workflowName) &&
                            activityName != null && String.Equals(activityName, currentActivityLongName))
                        {
                            if (workflowActionName != null)
                            {
                                //NOTE : if "WorkflowActionName" exists, it should validate agaist the action name
                                if ((String.Equals(workflowActionName, workflowAction) || String.Equals(workflowActionName, "[RSALL]")))
                                {
                                    //Options:
                                    //      [RSALL] -> Applicable for all actions (default value)
                                    //      Blank Value -> not Applicable for any actions
                                    //      Action Name -> Only Applicable for a particular action
                                    filteredStateviewWorkflowDepedancyRows.Add(row);
                                }
                            }
                            else
                            {
                                //NOTE : For Release Assurance, if "WorkflowActionName" doesn't exist it should work as before
                                filteredStateviewWorkflowDepedancyRows.Add(row);
                            }
                        }
                    }
                }

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: WorkflowName {0}, activity {1}, filetered records {2}", workflowName, currentActivityLongName, (filteredStateviewWorkflowDepedancyRows != null) ? filteredStateviewWorkflowDepedancyRows.Count : 0), MDMTraceSource.AdvancedWorkflow);
                }

                //Get the StateView Attributes Associated with filtered state views for the workflow and activity
                var svAttributesToBeChecked = (from svdf in filteredStateviewWorkflowDepedancyRows
                                               from sv in stateviewLookupRows
                                               let turl = sv.GetValue("TemplateURL")
                                               let wfName = svdf.GetValue("WorkflowName")
                                               let svName = svdf.GetValue("StateViewName")
                                               let svValue = sv.GetValue("Value")
                                               where (turl != null) && (wfName != null) && (svValue != null) && (turl.ToString().Contains(svName.ToString())) && String.Compare(svValue.ToString(), "Valid", StringComparison.InvariantCultureIgnoreCase) == 0
                                               select new
                                               {
                                                   Id = sv.GetValue("Id").ToString(),
                                                   Name = sv.GetValue("Name").ToString(),
                                                   StateViewName = svName.ToString(),
                                                   Url = turl
                                               }).Distinct().ToList();

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: Attributes to be checked {0}", (svAttributesToBeChecked != null) ? svAttributesToBeChecked.Count() : 0), MDMTraceSource.AdvancedWorkflow);
                }

                var stateviewAttributesUQList = new List<AttributeUniqueIdentifier>();

                svAttributesToBeChecked.ForEach((svac) =>
                {
                    stateviewAttributesUQList.Add((AttributeUniqueIdentifier)MDMObjectFactory.GetIAttributeUniqueIdentifier(svac.Name, Constants.STATE_VIEW_ATTRIBUTES_GROUP_NAME));
                });

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: Attributes to be checked {0}", stateviewAttributesUQList.Select(auid => auid.AttributeName).Aggregate((cur, next) => cur + ", " + next)), MDMTraceSource.AdvancedWorkflow);
                }

                var ensure = _entityManager.EnsureAttributes(eventEntities, stateviewAttributesUQList, false, callContext);

                if (Constants.TRACING_ENABLED)
                {
                    MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("MDM.EntityWorkflowManager.Business.EntityWorkflowBL.ValidateStateViews: Ensured State View Attributes on entity collection with entities {0}", eventEntities.Select(x => x.Id.ToString()).ToList().Aggregate((cur, next) => cur + "," + next)), MDMTraceSource.AdvancedWorkflow);
                }

                //Get the entities for which the stateview attributes are in invalid state and flag them  
                var entityList = eventEntities.ToList();

                var numberOfThreads = Environment.ProcessorCount * 2;

                var batchSize = (Int32)eventEntities.Count() / numberOfThreads;
                if (batchSize == 0)
                {
                    batchSize = 100;
                }
                var entc = Partitioner.Create(0, eventEntities.Count(), batchSize);
                var result = Parallel.ForEach(entc, entityBatch =>
                {
                    for (Int32 i = entityBatch.Item1; i < entityBatch.Item2; i++)
                    {
                        var ent = entityList[i];
                        try
                        {
                            var invalidattrs = from at in ent.Attributes
                                               join svat in svAttributesToBeChecked on at.Name equals svat.Name
                                               let value = at.GetCurrentValue() ?? "0"
                                               where Int32.Parse(value.ToString()) != Int32.Parse(svat.Id.ToString())
                                               select new
                                               {
                                                   EntityId = ent.Id,
                                                   EntityName = ent.Name,
                                                   AttributeId = at.Id,
                                                   AttributeName = at.Name,
                                                   AttributeValue = value,
                                                   StateViewName = svat.StateViewName
                                               };

                            if (Constants.TRACING_ENABLED)
                            {
                                MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("Trying to validate stateviews - Invalid Attributes {0}", (invalidattrs != null) ? invalidattrs.Count() : 0), MDMTraceSource.AdvancedWorkflow);
                            }

                            // Example of the format String 
                            if (invalidattrs.Any())
                            {
                                var operationResult = entityOperationResults.GetEntityOperationResult(ent.Id);
                                var parameters = new Collection<Object>() { invalidattrs.Select(a => a.StateViewName).Aggregate<String>((ws, next) => ws + "," + next), ent.Name };
                                operationResult.AddOperationResult("113988", String.Format("Invalid state views '{0}' for entity: '{1}'. Check and try again.", parameters.ToArray()), parameters, OperationResultType.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Exception occurred during validating State Views", ex);
                        }
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }

            entityOperationResults.RefreshOperationResultStatus();

            return EntityWorkflowOperationsCommonUtility.ScanAndFilterEntities(eventEntities, entityOperationResults);
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entities"></param>
        //private void UpdateDenorm(EntityCollection entities)
        //{
        //    DurationHelper durationHelper = new DurationHelper(DateTime.Now);
        //    EntityBL entityBL = new EntityBL();

        //    foreach (Entity entity in entities)
        //    {
        //        DenormEntityBL denormManager = new DenormEntityBL(entityBL);
        //        Entity dniENtity = denormManager.RefreshWorkflowAttributes(entity.Id, entity.ContainerId, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Denorm));

        //        //Disabled population of workflow attributes to dn_search table since search procedure takes data directly from core tables.
        //        //if (dniENtity != null)
        //        //{
        //        //    EntitySearchDataBL searchManager = new EntitySearchDataBL(entityBL);
        //        //    var result = searchManager.RefreshWorkflowAttributes(dniENtity, new CallerContext(MDMCenterApplication.MDMCenter, MDMCenterModules.Denorm));
        //        //}
        //    }

        //    if (Constants.PERFORMANCE_TRACING_ENABLED || Constants.TRACING_ENABLED)
        //    {
        //        MDMTraceHelper.EmitTraceEvent(TraceEventType.Information, String.Format("{0}ms - EntityWorkFlowBL-Method:UpdateDenorm", durationHelper.GetDurationInMilliseconds(DateTime.Now)), MDMTraceSource.DenormProcess);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="activityShortName"></param>
        /// <param name="workflowAction"></param>
        /// <param name="comments"></param>
        /// <param name="operationType"></param>
        /// <param name="securityUser"></param>
        /// <param name="callerContext"></param>
        /// <param name="workflowName"></param>
        /// <param name="workflowVersion"></param>
        /// <returns></returns>
        private EntityOperationResultCollection PerformBatchWorkflowAction(EntityCollection entities, String activityShortName, String workflowAction, String comments, String operationType, SecurityUser securityUser, CallerContext callerContext, String workflowName = "", Int32 workflowVersion = 0)
        {
            EntityOperationResultCollection masterEntityOperationResult = new EntityOperationResultCollection();
            EntityOperationResultCollection batchEntityOperationResult = new EntityOperationResultCollection();

            // Read batch size from MDMCenter.Entity.WorkflowTransition.BatchSize
            Int32 batchSize = AppConfigurationHelper.GetAppConfig<Int32>("MDMCenter.Entity.WorkflowTransition.BatchSize", 0);
            Int32 startIndex = 0;

            // If BatchSize is 0, set it to 100.
            if (batchSize == 0)
            {
                batchSize = 100;
            }

            // Start Batching
            while (startIndex < entities.Count)
            {
                Int32 endIndex = (startIndex + batchSize) - 1;

                if (endIndex >= entities.Count)
                {
                    endIndex = entities.Count - 1;
                }

                // Create a sub EntityCollection with number of entities equal to batch size
                EntityCollection batchEntities = new EntityCollection();

                // Take entities between startIndex and endIndex from entity identifier list
                for (Int32 i = startIndex; i <= endIndex; i++)
                {
                    Entity batchEntity = entities.ElementAt(i);
                    if (batchEntity != null)
                    {
                        batchEntities.Add(batchEntity);
                    }
                }

                // Make service call to resume workflow for a batch of entities
                try
                {
                    batchEntityOperationResult = new EntityOperationResultCollection();

                    if (batchEntities == null)
                    {
                        throw new Exception("No entities found in Entity collection.");
                    }

                    if (operationType == "ResumeWorkflow")
                    {
                        batchEntityOperationResult = ResumeWorkflow(batchEntities, workflowName, workflowVersion, activityShortName, workflowAction, comments, callerContext);
                    }
                    else if (operationType == "ChangeAssignment")
                    {
                        batchEntityOperationResult = ChangeAssignment(batchEntities, activityShortName, securityUser, workflowAction, callerContext);
                    }
                }
                catch (Exception ex)
                {
                    // If any exception occurs, add it to the masterOperationResult
                    masterEntityOperationResult.AddOperationResult("", "Error occurred: " + ex.Message, OperationResultType.Error);
                }
                finally
                {
                    // Add all EntityOperationResults from batchEntityOperationResult to masterEntityOperationResult
                    if (batchEntityOperationResult != null)
                    {
                        foreach (EntityOperationResult entityOperationResult in batchEntityOperationResult)
                        {
                            #region Update Error Messages

                            if (entityOperationResult.Errors != null)
                            {
                                entityOperationResult.Errors = UpdateErrors(entityOperationResult.Errors, masterEntityOperationResult, callerContext, false, entityOperationResult.EntityLongName);
                            }

                            #endregion

                            #region Update Information Messages

                            if (entityOperationResult.Informations != null)
                            {
                                entityOperationResult.Informations = UpdateInformations(entityOperationResult.Informations, masterEntityOperationResult, callerContext, false, entityOperationResult.EntityLongName);
                            }

                            #endregion

                            #region Update Warning Messages

                            if (entityOperationResult.Warnings != null)
                            {
                                entityOperationResult.Warnings = UpdateWarnings(entityOperationResult.Warnings, masterEntityOperationResult, callerContext, false, entityOperationResult.EntityLongName);
                            }
                            #endregion

                            masterEntityOperationResult.Add(entityOperationResult);
                        }
                        // Copy over Errors from batchEntityOperationResult to masterEntityOperationResult
                        if (batchEntityOperationResult.Errors != null)
                        {
                            batchEntityOperationResult.Errors = UpdateErrors(batchEntityOperationResult.Errors, masterEntityOperationResult, callerContext, true);
                        }

                        if (batchEntityOperationResult.Informations != null)
                        {
                            batchEntityOperationResult.Informations = UpdateInformations(batchEntityOperationResult.Informations, masterEntityOperationResult, callerContext, true);
                        }

                        if (batchEntityOperationResult.Warnings != null)
                        {
                            batchEntityOperationResult.Warnings = UpdateWarnings(batchEntityOperationResult.Warnings, masterEntityOperationResult, callerContext, true);
                        }
                    }
                }
                // Change start index
                startIndex = startIndex + batchSize;
            }

            // Refresh OperationResultStatus after all batches are processed
            masterEntityOperationResult.RefreshOperationResultStatus();

            return masterEntityOperationResult;
        }

        private ErrorCollection UpdateErrors(ErrorCollection errors, EntityOperationResultCollection masterEntityOperationResult, CallerContext callerContext, Boolean addErrorsInMaster = false, String entityName = "")
        {
            ErrorCollection errorsOperationResult = errors;

            #region Update Error Messages

            String errorCodeMessage = String.Empty;

            foreach (Error error in errorsOperationResult)
            {
                if (error.ErrorCode != null && String.IsNullOrWhiteSpace(error.ErrorMessage))
                {
                    // If error contains params then concatenate params to the message in messagecode
                    // Otherwise just consider message in messageCode
                    if (error.Params != null && error.Params.Count > 0)
                    {
                        Object[] parameters = ValueTypeHelper.ConvertObjectCollectionToArray(error.Params);
                        errorCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, parameters, false, callerContext).ToString();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(entityName))
                        {
                            errorCodeMessage = String.Format("{0} : {1}", entityName, _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext).ToString());
                        }
                        else
                        {
                            errorCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), error.ErrorCode, false, callerContext).ToString();
                        }
                    }
                }
                else if (!String.IsNullOrWhiteSpace(error.ErrorMessage))
                {
                    if (!String.IsNullOrEmpty(entityName))
                    {
                        errorCodeMessage = String.Format("{0} : {1}", entityName, error.ErrorMessage);
                    }
                    else
                    {
                        errorCodeMessage = error.ErrorMessage;
                    }
                }

                error.ErrorMessage = errorCodeMessage;

                if (addErrorsInMaster)
                {
                    masterEntityOperationResult.Errors.Add(error);
                }
            }

            #endregion

            return errorsOperationResult;
        }

        private InformationCollection UpdateInformations(InformationCollection informations, EntityOperationResultCollection masterEntityOperationResult, CallerContext callerContext, Boolean addInfosInMaster = false, String entityName = "")
        {
            InformationCollection informationsOperationResult = informations;

            #region Update Information Messages

            String informationCodeMessage = String.Empty;

            foreach (Information info in informationsOperationResult)
            {
                if (info.InformationCode != null && String.IsNullOrWhiteSpace(info.InformationMessage))
                {
                    // If info param is available then add to the information message.
                    if (info.Params != null && info.Params.Count > 0)
                    {
                        Object[] parameters = ValueTypeHelper.ConvertObjectCollectionToArray(info.Params);
                        informationCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), info.InformationCode, parameters, false, callerContext).ToString();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(entityName))
                        {
                            informationCodeMessage = String.Format("{0} : {1}", entityName, _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), info.InformationCode, false, callerContext).ToString());
                        }
                        else
                        {
                            informationCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), info.InformationCode, false, callerContext).ToString();
                        }
                    }
                }
                else if (!String.IsNullOrWhiteSpace(info.InformationMessage))
                {
                    if (!String.IsNullOrEmpty(entityName))
                    {
                        informationCodeMessage = String.Format("{0} : {1}", entityName, info.InformationMessage);
                    }
                    else
                    {
                        informationCodeMessage = info.InformationMessage;
                    }
                }

                info.InformationMessage = informationCodeMessage;

                if (addInfosInMaster)
                {
                    masterEntityOperationResult.Informations.Add(info);
                }
            }

            #endregion

            return informationsOperationResult;
        }

        private WarningCollection UpdateWarnings(WarningCollection warnings, EntityOperationResultCollection masterEntityOperationResult, CallerContext callerContext, Boolean addWarningsInMaster = false, String entityName = "")
        {
            WarningCollection warningsOperationResult = warnings;

            #region Update Warning Messages

            String warningCodeMessage = String.Empty;

            foreach (Warning warning in warningsOperationResult)
            {
                if (warning.WarningCode != null && String.IsNullOrWhiteSpace(warning.WarningMessage))
                {

                    if (warning.Params != null && warning.Params.Count > 0)
                    {
                        Object[] parameters = ValueTypeHelper.ConvertObjectCollectionToArray(warning.Params);
                        warningCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), warning.WarningCode, parameters, false, callerContext).ToString();
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(entityName))
                        {
                            warningCodeMessage = String.Format("{0} : {1}", entityName, _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), warning.WarningCode, false, callerContext).ToString());
                        }
                        else
                        {
                            warningCodeMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), warning.WarningCode, false, callerContext).ToString();
                        }
                    }
                }
                else if (!String.IsNullOrWhiteSpace(warning.WarningMessage))
                {
                    if (!String.IsNullOrEmpty(entityName))
                    {
                        warningCodeMessage = String.Format("{0} : {1}", entityName, warning.WarningMessage);
                    }
                    else
                    {
                        warningCodeMessage = warning.WarningMessage;
                    }
                }

                warning.WarningMessage = warningCodeMessage;

                if (addWarningsInMaster)
                {
                    masterEntityOperationResult.Warnings.Add(warning);
                }
            }

            #endregion

            return warningsOperationResult;
        }

        private EntityOperationResultCollection MergeEntityOperationResultCollections(List<EntityOperationResultCollection> operationsResults, CallerContext callerContext)
        {
            EntityOperationResultCollection mergedEntityOperationResult = new EntityOperationResultCollection();
            OperationResultStatusEnum commonOperationResultStatus = OperationResultStatusEnum.Successful;
            foreach (EntityOperationResultCollection entityOperationResults in operationsResults)
            {
                if (entityOperationResults.OperationResultStatus == OperationResultStatusEnum.Successful &&
                    entityOperationResults.Errors.Count > 0)
                {
                    Error error = new Error();
                    error.ErrorMessage = _localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "113824", false, callerContext).ToString();
                    // Few entities failed while transitioning in next activity in workflow. Please contact administrator.
                    mergedEntityOperationResult.Errors.Add(error);
                    commonOperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                }
                else
                {
                    foreach (EntityOperationResult entityOperationResult in entityOperationResults)
                    {
                        if (entityOperationResult.OperationResultStatus != OperationResultStatusEnum.Successful)
                        {
                            commonOperationResultStatus = OperationResultStatusEnum.CompletedWithErrors;
                        }

                        foreach (Information information in entityOperationResult.GetInformation())
                        {
                            mergedEntityOperationResult.Informations.Add(information);
                        }

                        foreach (Error error in entityOperationResult.GetErrors())
                        {
                            mergedEntityOperationResult.Errors.Add(error);
                        }

                        foreach (Warning warning in entityOperationResult.GetWarnings())
                        {
                            mergedEntityOperationResult.Warnings.Add(warning);
                        }
                    }
                }
            }
            mergedEntityOperationResult.OperationResultStatus = commonOperationResultStatus;
            return mergedEntityOperationResult;
        }

        private void UpdateMasterEnitityOperationResult(EntityOperationResultCollection masterEntityOperationResults, EntityOperationResultCollection batchEntityOperationResult)
        {
            //Add all EntityOperationResults from batchEntityOperationResult to masterEntityOperationResult
            if (batchEntityOperationResult != null)
            {
                foreach (EntityOperationResult entityOperationResult in batchEntityOperationResult)
                {
                    masterEntityOperationResults.Add(entityOperationResult);
                }

                //Copy over Errors from batchEntityOperationResult to masterEntityOperationResult
                if (batchEntityOperationResult.Errors != null)
                {
                    foreach (Error error in batchEntityOperationResult.Errors)
                    {
                        masterEntityOperationResults.Errors.Add(error);
                    }
                }

                if (batchEntityOperationResult.Informations != null)
                {
                    foreach (Information info in batchEntityOperationResult.Informations)
                    {
                        masterEntityOperationResults.Informations.Add(info);
                    }
                }

                if (batchEntityOperationResult.Warnings != null)
                {
                    foreach (Warning warning in batchEntityOperationResult.Warnings)
                    {
                        masterEntityOperationResults.Warnings.Add(warning);
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
