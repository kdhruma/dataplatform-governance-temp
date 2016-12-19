using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MDM.JigsawIntegrationManager.JsonSerializers
{
    using DTO;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.Core.Extensions;
    using Utility;

    /// <summary>
    /// 
    /// </summary>
    public class MessageJsonGenerator10
    {
        #region Properties

        private static readonly IReadOnlyCollection<ObjectAction> ProcessedAttributeActions = new List<ObjectAction>
        {
            ObjectAction.Create,
            ObjectAction.Update,
            ObjectAction.Delete,
            ObjectAction.Read
        };

        #endregion

        /// <summary>
        /// Generates the message json.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="requestParams">The request parameters.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="returnRequest">The return request.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        public String GenerateMessageJson(List<IMessageBase> messages, RequestParams requestParams, JigsawIntegrationAppName appName = JigsawIntegrationAppName.manageGovernApp, Boolean? returnRequest = null, String sourceName = "")
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

            var jsonObj = new JObject();

            try
            {
                jsonObj.Add(new JProperty("appName", appName.ToString()));

                if (returnRequest != null && returnRequest.HasValue)
                {
                    jsonObj.Add(new JProperty("returnRequest", returnRequest.Value));
                }

                if (requestParams != null)
                {
                    jsonObj.Add("requestParams", JObject.FromObject(requestParams.Params, JigsawJsonSerializer.JsonSerializer));
                }

                var entitiesJArray = new JArray();

                if (!messages.IsNullOrEmpty())
                {
                    foreach (var message in messages)
                    {
                        var entityObj = new JObject();

                        entityObj.Add("eid", message.Eid);

                        #region entity info

                        if (message.EntityInfo != null)
                        {
                            var entityInfo = new JObject();

                            entityInfo.Add("entityType", message.EntityInfo.EntityType);
                            entityInfo.Add("defaultLocale", message.EntityInfo.DefaultLocale);

                            entityObj.Add("entityInfo", entityInfo);
                        }

                        #endregion

                        #region system info

                        if (message.SystemInfo != null)
                        {
                            var systemInfo = new JObject();

                            systemInfo.Add("tenantId", message.SystemInfo.TenantId);

                            entityObj.Add("systemInfo", systemInfo);
                        }

                        #endregion

                        #region attributes info

                        if (message.AttributesInfo != null)
                        {
                            entityObj.Add("attributesInfo", GetAttributesInfoJson(message, appName, sourceName));
                        }

                        #endregion

                        #region invalid attributes info

                        if (message.InvalidAttributesInfo != null)
                        {
                            entityObj.Add("invalidAttributesInfo", GetInvalidAttributesInfoJson(message));
                        }

                        #endregion

                        #region extended attributes info

                        JObject extendedAttributesInfo = GetExtendedAttributesInfoJson(message);

                        if (extendedAttributesInfo != null)
                        {
                            entityObj.Add("jsAttributesInfo", extendedAttributesInfo);
                        }

                        #endregion

                        entitiesJArray.Add(entityObj);
                    }
                }

                jsonObj.Add("entities", entitiesJArray);

                if (currentTraceSettings.IsBasicTracingEnabled && jsonObj != null)
                {
                    diagnosticActivity.LogMessageWithData("See 'View Data' for serialized to JSON data", jsonObj.ToString());
                }
            }
            catch (Exception ex)
            {
                if (!currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError("GenerateMessageJson failed with exception :" + ex.StackTrace);
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.Stop();
                }
            }

            return jsonObj.ToString();
        }

        #region attributesInfo helper methods


        /// <summary>
        /// Gets the attributes information json.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        private JObject GetAttributesInfoJson(IMessageBase message, JigsawIntegrationAppName appName, String sourceName)
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

            var attributesJObject = new JObject();
            try
            {
                var attributesData = message.AttributesInfo;

                if (attributesData != null)
                {
                    if (attributesData.ExternalId.HasValue && attributesData.ExternalId > 0)
                    {
                        attributesJObject.Add(new JProperty("externalId", attributesData.ExternalId));
                    }

                    if (!attributesData.Attributes.IsNullOrEmpty())
                    {
                        foreach (Attribute attribute in attributesData.Attributes)
                        {
                            String attributeName = attribute.Name.ToJsCompliant();
                            JProperty property = attributesJObject.Property(attributeName);

                            if (property == null)
                            {
                                attributesJObject.Add(new JProperty(attributeName, attribute.Value));
                            }
                            else
                            {
                                if (!currentTraceSettings.IsBasicTracingEnabled)
                                {
                                    diagnosticActivity = new DiagnosticActivity();
                                    diagnosticActivity.Start();
                                }

                                diagnosticActivity.LogMessageWithData(String.Format("The attribute {0} is already defined on the attributes json object ", attributeName), attributesJObject.ToJigsawString());
                            }
                        }
                    }

                    if (appName == JigsawIntegrationAppName.matchApp)
                    {
                        attributesJObject.Add(new JProperty("system", "MDMCenter"));
                        attributesJObject.Add(new JProperty("source", sourceName.ToJsCompliant()));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError("GetAttributesInfoJson failed with exception :" + ex.StackTrace);
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.Stop();
                }
            }

            return attributesJObject;
        }


        /// <summary>
        /// Gets the invalid attributes information json.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private JObject GetInvalidAttributesInfoJson(IMessageBase message)
        {
            var attributesJObject = new JObject();
            var attributesData = message.InvalidAttributesInfo;

            if (attributesData != null)
            {
                if (!attributesData.Attributes.IsNullOrEmpty())
                {
                    foreach (Attribute attribute in attributesData.Attributes)
                    {
                        attributesJObject.Add(new JProperty(attribute.Name.ToJsCompliant(), attribute.Value));
                    }
                }
            }

            return attributesJObject;
        }

        #endregion

        #region extended attributes info helper methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private JObject GetExtendedAttributesInfoJson(IMessageBase message)
        {
            Type messageType = message.GetType();

            JObject extendedAttributesInfoJObject = null;

            if (messageType.Equals(typeof(EntityMessage)))
            {
                var entityMessage = (EntityMessage)message;

                if (entityMessage != null && entityMessage.ExtendedAttributesInfo != null)
                {
                    extendedAttributesInfoJObject = GetEntityExtendedAttributesInfoJson(entityMessage);
                }
            }
            else if (messageType.Equals(typeof(EventMessage)))
            {
                var eventMessage = (EventMessage)message;

                if (eventMessage != null && eventMessage.ExtendedAttributesInfo != null)
                {
                    extendedAttributesInfoJObject = GetEventExtendedAttributesInfoJson(eventMessage);
                }
            }

            return extendedAttributesInfoJObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private JObject GetEntityExtendedAttributesInfoJson(EntityMessage message)
        {
            var extendedAttributesInfo = message.ExtendedAttributesInfo;

            var extendedAttributesInfoJObject = new JObject();

            if (extendedAttributesInfo.JsRelationship != null)
            {
                extendedAttributesInfoJObject.Add("jsRelationship", JObject.FromObject(extendedAttributesInfo.JsRelationship, JigsawJsonSerializer.JsonSerializer));
            }

            if (extendedAttributesInfo.JsChangeContext != null)
            {
                extendedAttributesInfoJObject.Add("jsChangeContext", JObject.FromObject(extendedAttributesInfo.JsChangeContext, JigsawJsonSerializer.JsonSerializer));
            }

            if (extendedAttributesInfo.JsWorkflow != null && extendedAttributesInfo.JsWorkflow.Workflows.Count > 0)
            {
                extendedAttributesInfoJObject.Add("jsWorkflow", GetWorkflowJson(extendedAttributesInfo.JsWorkflow));
            }

            if (extendedAttributesInfo.JsBusinessConditionsSummary != null)
            {
                extendedAttributesInfoJObject.Add("businessConditionsSummary", JObject.FromObject(extendedAttributesInfo.JsBusinessConditionsSummary, new JsonSerializer()));
            }

            if (extendedAttributesInfo.JsValidationStatesSummary != null)
            {
                extendedAttributesInfoJObject.Add("validationStatesSummary", JObject.FromObject(extendedAttributesInfo.JsValidationStatesSummary, new JsonSerializer()));
            }

            if (extendedAttributesInfo.JsValidationStates != null && extendedAttributesInfo.JsValidationStates.Count > 0)
            {
                var jsValidationStatesJObject = new JObject();

                foreach (var validationState in extendedAttributesInfo.JsValidationStates)
                {
                    jsValidationStatesJObject.Add(new JProperty(validationState.Name, validationState.Value));
                }

                extendedAttributesInfoJObject.Add("jsValidationStates", jsValidationStatesJObject);
            }

            return extendedAttributesInfoJObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private JObject GetEventExtendedAttributesInfoJson(EventMessage message)
        {
            var extendedAttributesInfo = message.ExtendedAttributesInfo;

            var extendedAttributesInfoJObject = new JObject();

            if (extendedAttributesInfo.JsRelationship != null)
            {
                extendedAttributesInfoJObject.Add("jsRelationship", JObject.FromObject(extendedAttributesInfo.JsRelationship, JigsawJsonSerializer.JsonSerializer));
            }

            if (extendedAttributesInfo.JsChangeContext != null)
            {
                extendedAttributesInfoJObject.Add("jsChangeContext", JObject.FromObject(extendedAttributesInfo.JsChangeContext, JigsawJsonSerializer.JsonSerializer));
            }

            return extendedAttributesInfoJObject;
        }

        #region workflow helper methods

        public JObject GetWorkflowJson(WorkflowInfo workflowInfo)
        {
            var workflows = workflowInfo.Workflows;

            JObject outerWorkflowsJObject = new JObject();

            if (workflowInfo.CurrentWorkflows != null)
            {
                outerWorkflowsJObject.Add("currentWorkflows", workflowInfo.CurrentWorkflows.ToJArray());
            }

            if (workflowInfo.CurrentStages != null)
            {
                outerWorkflowsJObject.Add("currentStages", workflowInfo.CurrentStages.ToJArray());
            }

            if (workflows != null && workflows.Count > 0)
            {
                foreach (Workflow workflow in workflows)
                {
                    if (workflow.WorkflowStages != null && workflow.WorkflowStages.Count > 0)
                    {
                        JObject workflowJObject = new JObject();

                        workflowJObject.Add("workflowName", workflow.WorkflowName);
                        workflowJObject.Add("workflowVersion", workflow.WorkflowVersion);


                        foreach (var workflowStage in workflow.WorkflowStages)
                        {
                            if (workflowStage != null && !String.IsNullOrWhiteSpace(workflowStage.StageName))
                            {
                                var stageName = String.Format("stage_{0}", workflowStage.StageName.ToJsCompliant("_"));
                                workflowJObject.Add(stageName, JObject.FromObject(workflowStage, JigsawJsonSerializer.JsonSerializer));
                            }
                        }
                        outerWorkflowsJObject.Add(workflow.WorkflowName.ToJsCompliant("_"), workflowJObject);
                    }
                }
            }

            return outerWorkflowsJObject;
        }

        #endregion

        #endregion

    }
}
