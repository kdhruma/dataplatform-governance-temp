using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.JigsawIntegrationManager.MessageProducers
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Core.Extensions;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.JigsawIntegrationManager.DTO;
    using MDM.Utility;

    /// <summary>
    /// 
    /// </summary>
    internal class EventMessageProducer10
    {
        #region Properties

        private const String _eventEventType = "governevent";

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<IMessageBase> GenerateEventMessage(List<EventDataPackage> eventDataPackages, CallerContext callerContext)
		{
			#region Diagnostics Initialization

			DiagnosticActivity diagnosticActivity = null;

			var currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();

			if (currentTraceSettings.IsBasicTracingEnabled)
			{
				diagnosticActivity = new DiagnosticActivity();
				diagnosticActivity.Start();
			}

			#endregion

			var eventMessages = new List<IMessageBase>();

			try
			{
				if (!eventDataPackages.IsNullOrEmpty())
				{
					var defaultCulture = GlobalizationHelper.GetSystemDataLocale().GetCultureName();

					foreach (var eventDataPackage in eventDataPackages)
					{
						var eventMessage = new EventMessage();
                        JToken entityIds = null;

						var sourceEntity = eventDataPackage.SourceEntity;

						if (sourceEntity == null)
						{
							//TODO:: throw error?
							continue;
						}

						eventMessage.Eid = Guid.NewGuid().ToString(); // Always create new guid for events

						#region Entity info and system info

						eventMessage.EntityInfo = new DTO.EntityInfo { DefaultLocale = defaultCulture, EntityType = _eventEventType };

						eventMessage.SystemInfo = new DTO.SystemInfo { TenantId = JigsawConstants.Tenant };

						#endregion

						#region attributes info

						var attributesInfo = new AttributesInfo { ExternalId = sourceEntity.Id };

						if (eventDataPackage.EventData != null)
						{
							#region event base data - static fields

							var attributes = CreateBaseDataAttributes(eventDataPackage);

							#endregion

							#region event specific data - IEventData

							var eventData = eventDataPackage.EventData;

							if (eventData != null)
							{
								Type typeForEventData = eventData.GetType();

								if (typeForEventData.Equals(typeof(WorkflowEventData)))
								{
									var workflowEventData = (WorkflowEventData)eventData;

									if (workflowEventData != null)
									{
										var workflowEventDataAttributes = CreateWorkflowEventDataAttributes(workflowEventData);

										if (workflowEventDataAttributes != null && workflowEventDataAttributes.Count > 0)
										{
											attributes.AddRange(workflowEventDataAttributes);
										}
									}
								}

								var exportEventData = eventData as ExportEventData;

								if (exportEventData != null)
								{
									var exportEventDataAttributes = CreateExportEventDataAttributes(exportEventData);

									if (exportEventDataAttributes != null && exportEventDataAttributes.Count > 0)
									{
										attributes.AddRange(exportEventDataAttributes);
									}

                                    if (exportEventData.NumberOfEntitiesExported > 1)
                                    {
                                        entityIds = new JArray(exportEventData.ExternalEntityShortNames);
                                    }
                                }
							}

							#endregion

							attributesInfo.Attributes = attributes;
							eventMessage.AttributesInfo = attributesInfo;
						}

						#endregion

						#region extended attributes info

						var extendedAttributesInfo = new EventExtendedAttributesInfo();

						extendedAttributesInfo.JsRelationship = ProducerHelper.CreateRelationship(sourceEntity);
						extendedAttributesInfo.JsChangeContext = ProducerHelper.CreateChangeContext(callerContext);


                        // Update entityId for ExportEvent
                        if (entityIds!= null && extendedAttributesInfo.JsRelationship != null)
                        {
                            extendedAttributesInfo.JsRelationship.EntityId = entityIds;
                        }

						eventMessage.ExtendedAttributesInfo = extendedAttributesInfo;

						#endregion

						eventMessages.Add((IMessageBase)eventMessage);
                        if (currentTraceSettings.IsBasicTracingEnabled)
                        {
                            diagnosticActivity.LogMessageWithData(String.Format("Event Data {0} Event JSON Message", eventDataPackage.ToJigsawString()), eventMessage.ToJigsawString());
                        }
                    }
				}
			}
			finally
			{
				if (currentTraceSettings.IsBasicTracingEnabled)
				{
					diagnosticActivity.Stop();
				}
			}

			return eventMessages;
		}

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventDataPackage"></param>
		/// <returns></returns>
		private List<DTO.Attribute> CreateBaseDataAttributes(EventDataPackage eventDataPackage)
        {
            var attributes = new List<DTO.Attribute>();

            attributes.Add(new DTO.Attribute { Name = "eventType", Value = eventDataPackage.EventType.ToString() });
            attributes.Add(new DTO.Attribute { Name = "eventSubType", Value = eventDataPackage.EventSubType.ToString() });
            attributes.Add(new DTO.Attribute { Name = "eventSourceName", Value = eventDataPackage.EventSourceName.ToString() });
            attributes.Add(new DTO.Attribute { Name = "eventGroupId", Value = eventDataPackage.EventGroupId.ToString() });
            attributes.Add(new DTO.Attribute { Name = "actingUser", Value = eventDataPackage.ActingUser.ToString() });
            attributes.Add(new DTO.Attribute { Name = "sourceTimestamp", Value = eventDataPackage.TimeStamp.ToString("O") });

            return attributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowEventData"></param>
        /// <returns></returns>
        private List<DTO.Attribute> CreateWorkflowEventDataAttributes(WorkflowEventData workflowEventData)
        {
            var attributes = new List<DTO.Attribute>();

            attributes.Add(new DTO.Attribute { Name = "workflowName", Value = workflowEventData.WorkflowName });
            attributes.Add(new DTO.Attribute { Name = "workflowVersion", Value = workflowEventData.WorkflowVersion });

            attributes.Add(new DTO.Attribute { Name = "workflowStartTime", Value = workflowEventData.WorkflowStartTime.ToString("O") });
            attributes.Add(new DTO.Attribute { Name = "workflowEndTime", Value = workflowEventData.WorkflowEndTime.ToString("O") });

            attributes.Add(new DTO.Attribute { Name = "workflowStageFrom", Value = workflowEventData.WorkflowStageFrom });
            attributes.Add(new DTO.Attribute { Name = "workflowStageTo", Value = workflowEventData.WorkflowStageTo });

            attributes.Add(new DTO.Attribute { Name = "workflowStageFromDateTime", Value = workflowEventData.WorkflowStageFromDateTime.ToString("O") });
            attributes.Add(new DTO.Attribute { Name = "workflowStageToDateTime", Value = workflowEventData.WorkflowStageToDateTime.ToString("O") });
            attributes.Add(new DTO.Attribute { Name = "workflowStageTotalTIme", Value = workflowEventData.WorkflowStageTotalTime});

            attributes.Add(new DTO.Attribute { Name = "workflowAssignedFrom", Value = workflowEventData.WorkflowAssignedFrom });
            attributes.Add(new DTO.Attribute { Name = "workflowAssignedTo", Value = workflowEventData.WorkflowAssignedTo });

            attributes.Add(new DTO.Attribute { Name = "workflowStageActionTaken", Value = workflowEventData.WorkflowStageActionTaken });
            attributes.Add(new DTO.Attribute { Name = "workflowStageActionComments", Value = workflowEventData.WorkflowStageActionComments });

            return attributes;
        }

		/// <summary>
		/// Transforms the Export event properties to DTO Attributes
		/// </summary>
		/// <param name="exportEventData"></param>
		/// <returns></returns>
	    private List<DTO.Attribute> CreateExportEventDataAttributes(ExportEventData exportEventData)
		{

			var attributes = new List<DTO.Attribute>();

			attributes.Add(new DTO.Attribute { Name = "exportProfile", Value = exportEventData.ExportProfileName });
			attributes.Add(new DTO.Attribute { Name = "numberOfEntitiesExported", Value = exportEventData.NumberOfEntitiesExported });

			attributes.Add(new DTO.Attribute {Name = "exportStartTime", Value = exportEventData.ExportStartTime.ToString("O")});
			attributes.Add(new DTO.Attribute { Name = "exportEndTime", Value = exportEventData.ExportEndTime.ToString("O") });

			if (exportEventData.NumberOfEntitiesExported <= 1)
			{
				attributes.Add(new DTO.Attribute { Name = "collaborationContainerEid", Value = exportEventData.CollaborationContainerEntityGuidId.FirstOrDefault()});
				attributes.Add(new DTO.Attribute { Name = "approvedContainerEid", Value = exportEventData.ApprovedContainerEntityGuidId.FirstOrDefault()});
				attributes.Add(new DTO.Attribute { Name = "collaborationContainerExternalId", Value = exportEventData.CollaborationContainerEntityExternalId.FirstOrDefault() });
				attributes.Add(new DTO.Attribute { Name = "approvedContainerExternalId", Value = exportEventData.ApprovedContainerEntityExternalId.FirstOrDefault() });
			}
			else if (exportEventData.NumberOfEntitiesExported > 1)
			{
                attributes.Add(new DTO.Attribute { Name = "collaborationContainerEid", Value = new JArray(exportEventData.CollaborationContainerEntityGuidId) });
				attributes.Add(new DTO.Attribute { Name = "approvedContainerEid", Value = new JArray(exportEventData.ApprovedContainerEntityGuidId) });
				attributes.Add(new DTO.Attribute { Name = "collaborationContainerExternalId", Value = new JArray(exportEventData.CollaborationContainerEntityExternalId) });
				attributes.Add(new DTO.Attribute { Name = "approvedContainerExternalId", Value = new JArray(exportEventData.ApprovedContainerEntityExternalId) });
			}

			return attributes;
		}

	    #endregion
		}
}
