using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.JigsawIntegrationManager
{
    using BusinessObjects.DQM;
    using Core.Extensions;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.Core;
    using MDM.JigsawIntegrationManager.DataPackages;
    using MDM.JigsawIntegrationManager.DTO;
    using MDM.JigsawIntegrationManager.JsonSerializers;
    using MDM.JigsawIntegrationManager.MessageProducers;
    using MDM.Utility;

    /// <summary>
    /// Represents class for transforming MDMCenter objects into Jigsaw compliant json
    /// </summary>
    public class JigsawTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityMessageDataPackage"></param>
        /// <param name="callerContext"></param>
        /// <param name="jigsawIntegrationAppName"></param>
        /// <returns></returns>
        public static String CreateEntityMessage(EntityMessageDataPackage entityMessageDataPackage, CallerContext callerContext, JigsawIntegrationAppName jigsawIntegrationAppName)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            String entityMessageJson = String.Empty;

            try
            {
                var entityMessageProducer10 = new EntityMessageProducer10();

                List<IMessageBase> entityMessages = entityMessageProducer10.GenerateEntityMessage(new List<EntityMessageDataPackage> { entityMessageDataPackage }, callerContext);

                if (entityMessages != null && entityMessages.Count > 0)
                {
                    var messageJsonGenerator10 = new MessageJsonGenerator10();

                    var requestParams = new RequestParams();
                    requestParams.Params = new Dictionary<String, Object>();

                    requestParams.Params.Add("entityAction", entityMessageDataPackage.Entity != null ? entityMessageDataPackage.Entity.Action.ToJigsawString() : String.Empty);

                    entityMessageJson = messageJsonGenerator10.GenerateMessageJson(entityMessages, requestParams, jigsawIntegrationAppName);
                }
            }
            catch (Exception ex)
            {
                if (!isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.StackTrace);

            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.LogData("Generated message", entityMessageJson);
                    diagnosticActivity.Stop();
                }
            }

            return entityMessageJson;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventDataPackage"></param>
        /// <param name="callerContext"></param>
        /// <param name="jigsawIntegrationAppName"></param>
        /// <returns></returns>
        public static String CreateEventMessage(EventDataPackage eventDataPackage, CallerContext callerContext, JigsawIntegrationAppName jigsawIntegrationAppName)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            String entityMessageJson = String.Empty;

            try
            {
                var eventMessageProducer10 = new EventMessageProducer10();

                List<IMessageBase> entityMessages = eventMessageProducer10.GenerateEventMessage(new List<EventDataPackage> { eventDataPackage }, callerContext);

                if (entityMessages != null && entityMessages.Count > 0)
                {
                    var messageJsonGenerator10 = new MessageJsonGenerator10();

                    var requestParams = new RequestParams { Params = new Dictionary<String, Object>() };

                    entityMessageJson = messageJsonGenerator10.GenerateMessageJson(entityMessages, requestParams, jigsawIntegrationAppName);

                    if (isTracingEnabled)
                    {
                        diagnosticActivity.LogMessageWithData(String.Format("Event Data Package {0} JSON Message", eventDataPackage.ToJigsawString()), entityMessageJson);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.StackTrace);

            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.LogData("Generated message", entityMessageJson);
                    diagnosticActivity.Stop();
                }
            }

            return entityMessageJson;
        }

        /// <summary>
        /// Creates the match request message json.
        /// </summary>
        /// <param name="entityMessageDataPackage">The entity message data package.</param>
        /// <param name="matchProfile">The match profile.</param>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns>
        /// Json String for the match request
        /// </returns>
        public static String CreateMatchRequestMessage(EntityMessageDataPackage entityMessageDataPackage, MatchingProfile matchProfile, String sourceName, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            String entityMessageJson = String.Empty;

            try
            {
                var entityMessageProducer10 = new EntityMessageProducer10();

                List<IMessageBase> entityMessages = entityMessageProducer10.GenerateEntityMessage(new List<EntityMessageDataPackage> { entityMessageDataPackage }, callerContext);

                if (entityMessages != null && entityMessages.Count > 0)
                {
                    var messageJsonGenerator10 = new MessageJsonGenerator10();

                    var requestParams = new RequestParams();
                    requestParams.Params = new Dictionary<String, Object>();

                    requestParams.Params.Add("locale", matchProfile.Locale.GetCultureName());
                    requestParams.Params.Add("maxNumberOfResults", matchProfile.MaxReturnRecords);

                    List<MatchRequestContextFilter> contextFilters = new List<MatchRequestContextFilter>();
                    Entity sourceEntity = entityMessageDataPackage.Entity;

                    if (matchProfile.ApplyContainerFilter)
                    {
                        contextFilters.Add(new MatchRequestContextFilter
                        {
                            Path = "jsAttributesInfo.jsRelationship.container",
                            Comparator = "=",
                            Value = new String[] { sourceEntity.ContainerName }
                        });
                    }

                    if (matchProfile.ApplyCategoryFilter)
                    {
                        String pattern = JigsawConstants.CategoryPathSeparator;

                        if (!String.IsNullOrEmpty(sourceEntity.CategoryPath) && sourceEntity.CategoryPath.Contains(@"#@#"))
                        {
                            pattern = @"#@#";
                        }

                        contextFilters.Add(new MatchRequestContextFilter
                        {
                            Path = "jsAttributesInfo.jsRelationship.categoryPath",
                            Comparator = "=",
                            Value = new String[]
                            {
                                String.Join("/", ValueTypeHelper.SplitStringToStringCollection(sourceEntity.CategoryPath, pattern, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(pathName => pathName.Trim())
                                    .ToList())
                            }
                        });
                    }

                    String defaultCulture = GlobalizationHelper.GetSystemDataLocale().GetCultureName(); // System attributes are always in SDL

                    //Deleted items should never be considered. Pending match review items is configurable from profile
                    contextFilters.Add(new MatchRequestContextFilter
                    {
                        Path = String.Format("attributesInfo.{0}.{1}", defaultCulture, SystemAttributes.LifecycleStatus.ToString().ToJsCompliant()),
                        Comparator = "!=",
                        Value = matchProfile.IncludePendingMatchReviewItems
                            ? new String[] {LifecycleStatusValues.MarkedForDeletion}
                            : new String[] {LifecycleStatusValues.PendingMatchReview, LifecycleStatusValues.MarkedForDeletion},
                        Extension = ".raw"
                    });

                    requestParams.Params.Add("contextFilters", contextFilters);

                    entityMessageJson = messageJsonGenerator10.GenerateMessageJson(entityMessages, requestParams, JigsawIntegrationAppName.matchApp, false, sourceName);
                }
            }
            catch (Exception ex)
            {
                if (!isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.StackTrace);
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.LogData("Generated message", entityMessageJson);
                    diagnosticActivity.Stop();
                }
            }

            return entityMessageJson;
        }

        /// <summary>
        /// Creates the application config manage message.
        /// </summary>
        /// <param name="appDataPackage">The application data package.</param>
        /// <param name="jigsawIntegrationAppName">Name of the jigsaw integration application.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public static String CreateAppConfigManageMessage(AppConfigManageDataPackage appDataPackage, JigsawIntegrationAppName jigsawIntegrationAppName, CallerContext callerContext)
        {
            #region Diagnostics Initialization

            DiagnosticActivity diagnosticActivity = null;

            var isTracingEnabled = MDMOperationContextHelper.GetCurrentTraceSettings().IsBasicTracingEnabled;

            if (isTracingEnabled)
            {
                diagnosticActivity = new DiagnosticActivity();
                diagnosticActivity.Start();
            }

            #endregion

            String entityMessageJson = String.Empty;

            try
            {
                var appManageMessageProducer10 = new AppConfigManageMessageProducer10();

                List<IMessageBase> entityMessages = appManageMessageProducer10.GenerateAppManageMessage(new List<AppConfigManageDataPackage> { appDataPackage }, callerContext);

                if (entityMessages != null && entityMessages.Count > 0)
                {
                    var messageJsonGenerator10 = new MessageJsonGenerator10();

                    var requestParams = new RequestParams { Params = new Dictionary<String, Object>() };

                    requestParams.Params.Add("entityAction", appDataPackage.Action.ToJigsawString());

                    entityMessageJson = messageJsonGenerator10.GenerateMessageJson(entityMessages, requestParams, jigsawIntegrationAppName);
                }
            }
            catch (Exception ex)
            {
                if (!isTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity();
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.StackTrace);
            }
            finally
            {
                if (diagnosticActivity != null)
                {
                    diagnosticActivity.LogData("Generated message", entityMessageJson);
                    diagnosticActivity.Stop();
                }
            }

            return entityMessageJson;
        }
    }
}
