using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.MessageBrokerManager
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.JigsawIntegrationManager;
    using MDM.Utility;
    using DP = MDM.JigsawIntegrationManager.DataPackages;

    /// <summary>
    /// 
    /// </summary>
    public class MessageBrokerHelper
    {

        /// <summary>
        /// Sends the entity message.
        /// </summary>
        /// <param name="entityMessageDataPackages">The entity message data packages.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="callerProcessType">Type of the caller process.</param>
        /// <param name="brokerType">Type of the broker.</param>
        /// <param name="userName">Name of the user.</param>
        public static void SendEntityMessage(List<DP.EntityMessageDataPackage> entityMessageDataPackages, CallerContext callerContext, JigsawCallerProcessType callerProcessType, JigsawIntegrationBrokerType brokerType, String userName)
        {
            if (JigsawConstants.IsJigsawIntegrationEnabled)
            {
                IIntegrationMessageBroker manager = MessageBrokerFactory.GetMessageBrokerManager(callerProcessType, brokerType);

                DiagnosticActivity diagnosticActivity = new DiagnosticActivity();

                DurationHelper durationToSendMessage = null;

                Boolean isBasicTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

                if (isBasicTracingEnabled)
                {
                    diagnosticActivity.Start();

                    diagnosticActivity.LogInformation("Started Transform & Sending");
                    durationToSendMessage = new DurationHelper(DateTime.Now);
                }

                if (entityMessageDataPackages == null || entityMessageDataPackages.Count == 0)
                {
                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("EntityMessageDataPackages are not present. Cannot send.");
                        diagnosticActivity.Stop();
                    }

                    return;
                }

                try
                {
                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Starting Transform.");
                    }

                    var entityJsons = new Dictionary<String, String>();

                    var appName = JigsawIntegrationAppName.manageGovernApp;

                    if (callerContext.AdditionalProperties == null)
                    {
                        callerContext.AdditionalProperties = new Dictionary<String, Object>();
                    }

                    if (!callerContext.AdditionalProperties.ContainsKey("UserName"))
                    {
                        callerContext.AdditionalProperties.Add("UserName", userName);
                    }

                    foreach(var entityMessageDataPackage in entityMessageDataPackages)
                    {
                        var entityGuid = ((entityMessageDataPackage.Entity != null) && (entityMessageDataPackage.Entity.EntityGuid.HasValue)) ? entityMessageDataPackage.Entity.EntityGuid.ToString() : Guid.NewGuid().ToString();

                        if (JigsawUtility.IsJigsawIntegrationEnabledForEntityType(entityMessageDataPackage.Entity.EntityTypeId))
	                    {
		                    if (!entityJsons.ContainsKey(entityGuid))
		                    {
			                    var entityJson = JigsawTransformer.CreateEntityMessage(entityMessageDataPackage, callerContext, appName);
			                    entityJsons.Add(entityGuid, entityJson);
		                    }
		                    else
		                    {
			                    //TODO: WHat to do if same entity is coming twice in same call..?
		                    }
	                    }
                    }

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed Transform.");
                    }

                    manager.SendEntities(entityJsons);

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed SendEntity.");
                        diagnosticActivity.LogDurationInfo("Completed Transform & Sending", durationToSendMessage.GetCumulativeTimeSpanInMilliseconds());
                    }
                }
                catch (Exception ex)
                {
                    if (!isBasicTracingEnabled)
                    {
                        diagnosticActivity.Start();
                    }

                    diagnosticActivity.LogError(ex.StackTrace);
                    // ToDo Prasad - throw here to propoage, once this feature is stable
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


        /// <summary>
        /// Sends the event message.
        /// </summary>
        /// <param name="eventDataPackages">The event data packages.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="callerProcessType">Type of the caller process.</param>
        /// <param name="brokerType">Type of the broker.</param>
        /// <param name="userName">Name of the user.</param>
        public static void SendEventMessage(List<DP.EventDataPackage> eventDataPackages, CallerContext callerContext, JigsawCallerProcessType callerProcessType, JigsawIntegrationBrokerType brokerType, String userName)
        {
            DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
            Boolean isBasicTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isBasicTracingEnabled)
            {
                diagnosticActivity.Start();
                diagnosticActivity.LogInformation("Send Event Message");
            }

            if (JigsawConstants.IsJigsawInformationGovernanceReportsEnabled)
            {
                IIntegrationMessageBroker manager = MessageBrokerFactory.GetMessageBrokerManager(callerProcessType, brokerType);

                DurationHelper durationToSendMessage = null;

                try
                {
                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Starting Transform.");
                        durationToSendMessage = new DurationHelper(DateTime.Now);
                    }

                    var eventJsons = new Dictionary<String, String>();

                    var appName = JigsawIntegrationAppName.manageGovernApp;

                    foreach(var eventInfo in eventDataPackages)
                    {
                        var eventJson = JigsawTransformer.CreateEventMessage(eventInfo, callerContext, appName);

                        if (!String.IsNullOrEmpty(eventJson))
                        {
                            eventJsons.Add(Guid.NewGuid().ToString(), eventJson);
                        }
                    }

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed Transform.");
                    }

                    if (eventJsons.Any())
                    {
                        manager.SendEntities(eventJsons);
                    }
                    else
                    {
                        if (isBasicTracingEnabled)
                        {
                            diagnosticActivity.Start();
                        }

                        diagnosticActivity.LogWarning("Event JSON list was empty. No message was sent.");
                    }

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed SendEventMessage");
                        diagnosticActivity.LogDurationInfo("Completed Transform & Sending", durationToSendMessage.GetCumulativeTimeSpanInMilliseconds());
                    }
                }
                catch (Exception ex)
                {
                    diagnosticActivity.LogError(ex.Message);
                    // ToDo Prasad - throw here to propoage, once this feature is stable
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

        /// <summary>
        /// Groups the entities into groups and sends the event messages.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="profileName"></param>
        /// <param name="eventguidId"></param>
        /// <param name="eventSubType"></param>
        /// <param name="originalEntityIds"></param>
        /// <param name="isApprovedCopy"></param>
        /// <param name="brokerType"></param>
        /// <param name="callerContext"></param> 
        public static void SendGroupedExportEventMessages(IList<Entity> entities,String profileName, Guid eventguidId, DP.EventSubType eventSubType, IDictionary<Int64, Int64> originalEntityIds, Boolean isApprovedCopy, JigsawIntegrationBrokerType brokerType, CallerContext callerContext)
        {
            var activity = new DiagnosticActivity();
            Boolean isBasicTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;
            try
            {
                if (isBasicTracingEnabled)
                {
                    activity.Start();
                }

                var entityGroupingList = (from entity in entities
                                          group entity by new {entity.EntityTypeName, entity.ContainerName, entity.CategoryPath}
                                          into groupedEntities
                                          select groupedEntities).ToList();

                foreach (var groupedEntityList in entityGroupingList)
                {
                    List<Int64> collaborationEntityExternalIdList = new List<Int64>();

                    if (isApprovedCopy)
                    {
                        if ((originalEntityIds != null) && (originalEntityIds.Any()))
                        {
                            collaborationEntityExternalIdList = groupedEntityList.Select(a => originalEntityIds[a.Id]).ToList();
                        }
                    }
                    else
                    {
                        collaborationEntityExternalIdList = groupedEntityList.Select(a => a.Id).ToList();
                    }

                    DP.ExportEventData exportEventData = new DP.ExportEventData()
                    {
                        ExportProfileName = profileName,
                        NumberOfEntitiesExported = groupedEntityList.Count(),
                        ApprovedContainerEntityExternalId = groupedEntityList.Any(a => (a.CrossReferenceId != 0)) ? groupedEntityList.Select(a => a.CrossReferenceId).ToList() : new List<Int64>(),
                        CollaborationContainerEntityExternalId = collaborationEntityExternalIdList,
                        ApprovedContainerEntityGuidId = groupedEntityList.Any(a => !String.IsNullOrEmpty(a.CrossReferenceGuid.ToString())) ? groupedEntityList.Where(a => !String.IsNullOrEmpty(a.CrossReferenceGuid.ToString())).Select(a => a.CrossReferenceGuid.ToString()).ToList() : new List<String>(),
                        CollaborationContainerEntityGuidId = groupedEntityList.Any(a => !String.IsNullOrEmpty(a.EntityGuid.ToString())) ? groupedEntityList.Where(a => !String.IsNullOrEmpty(a.EntityGuid.ToString())).Select(a => a.EntityGuid.ToString()).ToList() : new List<String>(),
                        ExternalEntityShortNames = groupedEntityList.Any(a => !String.IsNullOrEmpty(a.Name)) ? groupedEntityList.Where(a => !String.IsNullOrEmpty(a.Name)).Select(a => a.Name).ToList() : new List<String>()
                    };

                    String currentUser = GetCurrentUser(callerContext);

                    DP.EventDataPackage eventDataPackage = new DP.EventDataPackage()
                    {
                        SourceEntity = groupedEntityList.FirstOrDefault(),
                        EventType = DP.EventType.entityExport,
                        EventSubType = eventSubType,
                        ActingUser = currentUser,
                        TimeStamp = DateTime.Now,
                        EventSourceName = callerContext.Application.ToString(),
                        EventGroupId = eventguidId,
                        EventData = exportEventData
                    };

                    if (isBasicTracingEnabled)
                    {
                        activity.LogData("Export Event Datat", eventDataPackage.ToJigsawString());
                    }

                    MessageBrokerHelper.SendEventMessage(new List<DP.EventDataPackage> {eventDataPackage}, callerContext, JigsawCallerProcessType.ExportEvent, brokerType, currentUser);
                }
            }
            catch (Exception ex)
            {
                activity.LogError(ex.Message);
            }
            finally
            {
                if (isBasicTracingEnabled)
                {
                    activity.Stop();
                }
            }
        }

        /// <summary>
        /// Send a Export Job Status Message
        /// </summary>
        /// <param name="eventguidId"></param>
        /// <param name="profileName"></param>
        /// <param name="jobEventSubType"></param>
        /// <param name="jobStartTime"></param>
        /// <param name="jobTime"></param>
        /// <param name="itemsProcessed"></param>
        /// <param name="brokerType"></param>
        /// <param name="callerContext"></param>
		public static void SendExportJobStatusMessageToJigsaw(Guid eventguidId, String profileName, DP.EventSubType jobEventSubType,DateTime jobStartTime, DateTime jobTime, Int32 itemsProcessed, JigsawIntegrationBrokerType brokerType , CallerContext callerContext)
        {
            #region Send Started SubEvent to jigSaw

            DP.ExportEventData exportEventData = new DP.ExportEventData()
            {
                ExportProfileName = profileName,
                NumberOfEntitiesExported = itemsProcessed,
                ApprovedContainerEntityExternalId = new List<Int64>(),
                CollaborationContainerEntityExternalId = new List<Int64>(),
                ApprovedContainerEntityGuidId = new List<String>(),
                CollaborationContainerEntityGuidId = new List<String>(),
                ExportStartTime = jobStartTime
            };

            if (DP.EventSubType.entityExportEnd == jobEventSubType)
            {
                exportEventData.ExportEndTime = jobTime;
            }

            String currentUser = GetCurrentUser(callerContext);

            DP.EventDataPackage eventDataPackage = new DP.EventDataPackage()
            {
                SourceEntity = new Entity(),
                EventType = DP.EventType.entityExport,
                EventSubType = jobEventSubType,
                ActingUser = currentUser,
                TimeStamp = DateTime.Now,
                EventSourceName = callerContext.Application.ToString(),
                EventGroupId = eventguidId,
                EventData = exportEventData
            };

            MessageBrokerHelper.SendEventMessage(new List<DP.EventDataPackage> { eventDataPackage }, callerContext, JigsawCallerProcessType.ExportEvent, brokerType, currentUser);

            #endregion
        }

        /// <summary>
        /// Sends the application manage message.
        /// </summary>
        /// <param name="appDataPackages">The application data packages.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <param name="callerProcessType">Type of the caller process.</param>
        /// <param name="brokerType">Type of the broker.</param>
        public static void SendAppConfigManageMessage(List<DP.AppConfigManageDataPackage> appDataPackages, CallerContext callerContext, JigsawCallerProcessType callerProcessType, JigsawIntegrationBrokerType brokerType)
        {
            if (JigsawConstants.IsJigsawMatchingEnabled)
            {
                IIntegrationMessageBroker manager = MessageBrokerFactory.GetMessageBrokerManager(callerProcessType, brokerType);

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
                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Starting Transform.");
                    }

                    var appMessageJsons = new Dictionary<String, String>();

                    var appName = JigsawIntegrationAppName.appConfigManageApp;

                    foreach (var appPackage in appDataPackages)
                    {
                        var appMessageJson = JigsawTransformer.CreateAppConfigManageMessage(appPackage, appName, callerContext);
                        appMessageJsons.Add(Guid.NewGuid().ToString(), appMessageJson);
                    }

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed Transform.");
                    }

                    manager.SendEntities(appMessageJsons);

                    if (isBasicTracingEnabled)
                    {
                        diagnosticActivity.LogDurationInfo("Completed SendAppManageMessage");
                        diagnosticActivity.LogDurationInfo("Completed Transform & Sending", durationToSendMessage.GetCumulativeTimeSpanInMilliseconds());
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

        private static String GetCurrentUser(CallerContext callerContext)
        {
            String currentUser = SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserName;

            if ((callerContext.AdditionalProperties != null) && (callerContext.AdditionalProperties.ContainsKey("UserName")))
            {
                currentUser = callerContext.AdditionalProperties["UserName"].ToString();
            }

            return currentUser;
        }
    }
}