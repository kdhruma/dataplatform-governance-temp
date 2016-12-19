using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Globalization;


namespace MDM.Workflow.TrackingManager.Business
{
    using MDM.Core;
    using MDM.Utility;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.ConfigurationManager.Business;
    using MDM.Workflow.TrackingManager.Data;
    using MDM.Workflow.PersistenceManager.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.Interfaces;
    using MDM.JigsawIntegrationManager;
    using MDM.MessageBrokerManager;
    using MDM.EntityManager.Business;
    using MDM.Workflow.Designer.Business;

    /// <summary>
    /// 
    /// </summary>
    public sealed class JigsawIntegrationHelper
    {
        /// <summary>
        /// Send workflow message and events to jigsaw for given worktime runtime instance id and workflow status
        /// </summary>
        /// <param name="runtimeInstanceId">Defines workflow runtime instance id</param>
        /// <param name="status">Defines workflow status</param>
        /// <param name="callerContext">Defines caller context</param>
        public static void SendToJigsaw(String runtimeInstanceId, String status, CallerContext callerContext)
        {
            if (JigsawConstants.IsJigsawIntegrationEnabled)
            {
                var workflowEventType = GetApplicableWorkflowEventType(status);

                if (workflowEventType != EventType.none)
                {
                    DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                    DurationHelper durationToSendMessage = null;

                    Boolean isBasicTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.Start();

                        diagnosticActivity.LogInformation("Started Transform & Sending");
                        durationToSendMessage = new DurationHelper(DateTime.Now);
                    }

                    try
                    {

                        var workflowInstances = new WorkflowInstanceBL().GetByRuntimeInstanceIds(runtimeInstanceId, callerContext);
                        var workflowInstance = workflowInstances != null ? workflowInstances.FirstOrDefault() : null;

                        if (workflowInstance != null)
                        {
                            var dataLocales = new Collection<LocaleEnum> { GlobalizationHelper.GetSystemDataLocale() };
                            var entityContext = new EntityContext { LoadWorkflowInformation = true, LoadEntityProperties = true, DataLocales = dataLocales };

                            if (workflowInstance.WorkflowMDMObjects != null && workflowInstance.WorkflowMDMObjects.Count > 0)
                            {
                                var workflowVersion = new WorkflowVersionBL().GetById(workflowInstance.WorkflowVersionId, callerContext);

                                if (workflowVersion != null)
                                {
                                    var dummyWorkflowState = new WorkflowState
                                    {
                                        WorkflowId = workflowVersion.WorkflowId,
                                        WorkflowName = workflowVersion.WorkflowShortName,
                                        WorkflowLongName = workflowVersion.WorkflowLongName,
                                        WorkflowVersionId = workflowVersion.VersionNumber,
                                        WorkflowVersionName = workflowVersion.VersionName,
                                        ActivityShortName = JigsawConstants.DummyWorkflowActivityName,
                                        ActivityLongName = JigsawConstants.DummyWorkflowActivityName,
                                        InstanceId = workflowInstance.Id
                                    };

                                    foreach (var workflowMdmObject in workflowInstance.WorkflowMDMObjects)
                                    {
                                        var entity = new EntityBL().Get(workflowMdmObject.MDMObjectId, entityContext, callerContext.Application, callerContext.Module, false, false);

                                        if (entity != null)
                                        {
                                            entity.WorkflowStates.Add(dummyWorkflowState);
                                            entity.WorkflowActionContext = new WorkflowActionContext();
                                            SendToJigsaw(entity, dummyWorkflowState.WorkflowName, dummyWorkflowState.ActivityShortName, workflowEventType, callerContext);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            diagnosticActivity.LogError("Workflow instance not found with runtime instance id:" + runtimeInstanceId);
                        }

                    }
                    catch (Exception ex)
                    {
                        diagnosticActivity.LogError(ex.Message);
                    }
                    finally
                    {
                        if (isBasicTracingEnabled)
                        {
                            diagnosticActivity.Stop();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="currentWorkflowName"></param>
        /// <param name="currentActivityName"></param>
        /// <param name="workflowEventType"></param>
        /// <param name="callerContext"></param>
        public static void SendToJigsaw(Entity entity, String currentWorkflowName, String currentActivityName, EventType workflowEventType, CallerContext callerContext)
        {
            if (JigsawConstants.IsJigsawIntegrationEnabled)
            {
                var actingUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

                var performedAction = String.Empty;
                var actionContext = entity.WorkflowActionContext;
                var workflowStates = entity.WorkflowStates;

                WorkflowState currentWorkflowState = null;

                if (entity.WorkflowStates != null)
                {
                    if (!String.IsNullOrEmpty(currentActivityName) && !currentActivityName.Equals(JigsawConstants.DummyWorkflowActivityName))
                    {
                        currentWorkflowState = entity.WorkflowStates.FirstOrDefault(ws => (ws.WorkflowName.Equals(currentWorkflowName) && ws.ActivityShortName.Equals(currentActivityName)));
                    }

                    if (currentWorkflowState == null)
                    {
                        currentWorkflowState = entity.WorkflowStates.FirstOrDefault(ws => (ws.WorkflowName.Equals(currentWorkflowName)));
                    }

                    if (currentWorkflowState == null && workflowEventType == EventType.workflowCompleted)
                    {
                        currentWorkflowState = new WorkflowState
                        {
                            WorkflowName = currentWorkflowName,
                            WorkflowVersionName = String.Empty
                        };
                    }
                }

                #region Get acting user

                if (actionContext != null)
                {
                    if (!String.IsNullOrWhiteSpace(actionContext.ActingUserName))
                    {
                        actingUser = actionContext.ActingUserName;
                    }

                    if (!String.IsNullOrWhiteSpace(actionContext.UserAction))
                    {
                        performedAction = actionContext.UserAction;
                    }
                }

                #endregion

                #region Send workflow events

                if (workflowEventType == EventType.workflowTransitioned && String.IsNullOrEmpty(currentWorkflowState.PreviousActivityShortName))
                {
                    SendWorkflowEventToJigsaw(entity, currentWorkflowState, EventType.workflowStarted, actingUser, performedAction, callerContext);
                }

                SendWorkflowEventToJigsaw(entity, currentWorkflowState, workflowEventType, actingUser, performedAction, callerContext);

                #endregion

                #region Send entity message with workflow

                entity.Action = ObjectAction.Update;

                var entityMessageDataPackage = new EntityMessageDataPackage
                {
                    Entity = entity,
                    WorkflowStates = entity.WorkflowStates
                };

                MessageBrokerHelper.SendEntityMessage(new List<EntityMessageDataPackage> { entityMessageDataPackage }, callerContext, JigsawCallerProcessType.DataQualityMessage, JigsawConstants.IntegrationBrokerType, actingUser);

                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="currentWorkflowState"></param>
        /// <param name="workflowEventType"></param>
        /// <param name="actingUser"></param>
        /// <param name="performedAction"></param>
        /// <param name="callerContext"></param>
        private static void SendWorkflowEventToJigsaw(Entity entity, IWorkflowState currentWorkflowState, EventType workflowEventType, String actingUser, String performedAction, CallerContext callerContext)
        {
            if (currentWorkflowState != null)
            {
                var eventDataPackage = new EventDataPackage
                {
                    EventType = workflowEventType,
                    EventSubType = EventSubType.none,
                    ActingUser = actingUser,
                    SourceEntity = entity,
                    TimeStamp = DateTime.Now,
                    EventSourceName = String.Empty,
                    EventGroupId = null,
                };

                var workflowEventData = new WorkflowEventData
                {
                    WorkflowName = currentWorkflowState.WorkflowName,
                    WorkflowVersion = currentWorkflowState.WorkflowVersionName,

                    WorkflowStageFrom = currentWorkflowState.PreviousActivityLongName,
                    WorkflowStageTo = currentWorkflowState.ActivityLongName.Equals(JigsawConstants.DummyWorkflowActivityName) ? String.Empty : currentWorkflowState.ActivityLongName,

                    WorkflowAssignedFrom = currentWorkflowState.PreviousActivityUser,
                    WorkflowAssignedTo = currentWorkflowState.AssignedUser,

                    WorkflowStageActionTaken = String.IsNullOrWhiteSpace(performedAction) ? currentWorkflowState.PreviousActivityAction : performedAction,
                    WorkflowStageActionComments = currentWorkflowState.PreviousActivityComments,

                    WorkflowStartTime = DateTime.Now,
                    WorkflowEndTime = DateTime.Now,

                    WorkflowStageFromDateTime = ValueTypeHelper.DateTimeTryParse(currentWorkflowState.PreviousActivityEventDate, DateTime.Now, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                    WorkflowStageToDateTime = ValueTypeHelper.DateTimeTryParse(currentWorkflowState.EventDate, DateTime.Now, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal) 
                };

                if (workflowEventType == EventType.workflowTransitioned)
                {
                    if (!String.IsNullOrEmpty(currentWorkflowState.PreviousActivityEventDate) && !String.IsNullOrEmpty(currentWorkflowState.EventDate))
                    {
                        workflowEventData.WorkflowStageTotalTime = workflowEventData.WorkflowStageToDateTime.Subtract(workflowEventData.WorkflowStageFromDateTime).TotalSeconds;
                    }
                }

                eventDataPackage.EventData = workflowEventData;

                MessageBrokerHelper.SendEventMessage(new List<EventDataPackage> { eventDataPackage }, callerContext, JigsawCallerProcessType.WorkflowEvent, JigsawConstants.IntegrationBrokerType, actingUser);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowStatus"></param>
        /// <returns></returns>
        private static EventType GetApplicableWorkflowEventType(String workflowStatus)
        {
            var eventType = EventType.none;

            switch(workflowStatus)
            {
                case "Completed":
                    eventType = EventType.workflowCompleted;
                    break;
                case "Terminated":
                case "Aborted":
                    eventType = EventType.workflowTerminated;
                    break;
                default:
                    eventType = EventType.none;
                    break;
            }

            return eventType;
        }
    }
}
