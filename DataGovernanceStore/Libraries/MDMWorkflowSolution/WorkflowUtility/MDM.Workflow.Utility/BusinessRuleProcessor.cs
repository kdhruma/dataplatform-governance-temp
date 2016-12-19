using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Reflection;

using Microsoft.Practices.ServiceLocation;

namespace MDM.Workflow.Utility
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Workflow;
    using MDM.Interfaces;
    using MDM.ExceptionManager;
    using MDM.Core;
	using MDM.BusinessObjects.Diagnostics;
	using MDM.BusinessObjects.DQM;
	using MDM.BusinessObjects.DQMMerging;
	using MDM.Utility;

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public class BusinessRuleProcessor
    {
        #region Fields

        #endregion

        #region Property

        /// <summary>
        /// The xml document representing details of a business rule
        /// </summary>
        public XmlDocument RuleXmlDocument { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Workflow Process
        /// </summary>
        /// <param name="context">Workflow Business Rule Context</param>
        /// <param name="assemblyPath">Name of the assembly path</param>
        /// <param name="assemblyName">Name of the assembly Name</param>
        /// <param name="typeName">Name of the method to invoke</param>
        /// <param name="operationResult">Operation result</param>
        /// <returns>Return OperationResult</returns>
        public OperationResult ProcessRules(WorkflowBusinessRuleContext context, String assemblyPath, String assemblyName, String typeName, OperationResult operationResult)
        {
            try
            {
                Type type;

                Assembly asm = Assembly.LoadFile(Path.Combine(assemblyPath, assemblyName));
                type = asm.GetType(typeName);

                if (type == null)
                {
                    throw new Exception("Can not find business rule class " + typeName + " in business rule assembly.");
                }

                var processor = GetProcessor(type);

                processor.Process(context, operationResult);
            }
            catch (FileNotFoundException ex)
            {
                operationResult.Errors.Add(
                    new Error("",
                              String.Format("Error executing business rule.\n\r{0}", "The assembly file specified in business rule xml can not be found at BusinessRule.PluginPath location.\n\rPlease check the configuration and verify the assembly file exists.")));
                //it logs in EventViewer, if configured
                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            catch (FileLoadException ex)
            {
                operationResult.Errors.Add(
                    new Error("",
                              String.Format("Error executing business rule.\n\r{0}", "Found the assembly for business rule but can not load the class to execute business rule. Check if any dependent assembly is missing.")));

                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            catch (BadImageFormatException ex)
            {
                operationResult.Errors.Add(
                    new Error("",
                              String.Format("Error executing business rule.\n\r{0}", "The assembly specified for business rule can not be loaded. Probable cause can be the framework version mismatch.")));

                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            catch (ArgumentException ex)
            {
                operationResult.Errors.Add(
                    new Error("",
                              String.Format("Error executing business rule.\n\r{0}", "Found the assembly for business rule but can not load the class because of the argument mismatch.")));

                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }
            catch (Exception ex)
            {
                operationResult.Errors.Add(
                    new Error("",
                              String.Format("Error executing business rule.\n\r{0}", ex.Message)));

                ExceptionHandler exHandler = new ExceptionHandler(ex);
            }

            return operationResult;
        }

        /// <summary>
        /// Call to add the workflow action to Asyn process queue.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="actionContext"></param>
        /// <param name="workflowVersion"></param>
        /// <param name="activityName"></param>
        /// <param name="workflowRuntimeInstanceId"></param>
        /// <param name="activityList"></param>
        /// <returns></returns>
        public static OperationResult CallAsyncProcess(Int64 entityId, WorkflowActionContext actionContext, Int64 workflowVersion, String activityName, String workflowRuntimeInstanceId, EntityActivityList activityList)
        {
            EntityFamilyQueue entityFamilyQueue = GetEntityFamilyQueueWithWorkflowChangeContext(entityId, actionContext, workflowVersion, activityName, workflowRuntimeInstanceId, activityList, "BusinessRuleProcessor.CallAsyncProcess");
            IEntityFamilyQueueManager entityFamilyQueueManager = ServiceLocator.Current.GetInstance(typeof(IEntityFamilyQueueManager)) as IEntityFamilyQueueManager;

            return entityFamilyQueueManager.Process(entityFamilyQueue, new CallerContext(MDMCenterApplication.WindowsWorkflow, MDMCenterModules.MDMAdvanceWorkflow));
        }

        /// <summary>
        /// Evaluates the Skip/Exit criteria for an workflow activity by calling the DDG Module
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="actionContext"></param>
        /// <param name="workflowVersion"></param>
        /// <param name="activityName"></param>
        /// <param name="workflowRuntimeInstanceId"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static Boolean EvaluateActivitySkipCriteria(Int64 entityId, WorkflowActionContext actionContext, Int64 workflowVersion, String activityName, String workflowRuntimeInstanceId, CallerContext callerContext)
        {
            //var skipActivity = false;

            //EntityFamilyQueue entityFamilyQueueItem = GetEntityFamilyQueueWithWorkflowChangeContext(entityId, actionContext, workflowVersion, activityName, workflowRuntimeInstanceId, EntityActivityList.UnKnown, "BusinessRuleProcessor.EvaluateActivitySkipCriteria");

            //EntityOperationResultCollection entityOperationResultcollection = WorkflowHelper.EvaluateWorkflowBusinessRules(entityFamilyQueueItem, MDMEvent.WorkflowActivityEntryCriteria, callerContext);

            //if ((entityOperationResultcollection != null) && entityOperationResultcollection.Any())
            //{
            //    EntityOperationResult result = (EntityOperationResult)entityOperationResultcollection.GetByEntityId(entityId);
            //    if ((result != null) && (result.HasError))
            //    {
            //        skipActivity = true;
            //    }
            //}

            //return skipActivity;

            return false;
        }

        /// <summary>
        /// Evaluates the Entry critera for an workflow activity by calling the DDG Module
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="actionContext"></param>
        /// <param name="workflowVersion"></param>
        /// <param name="activityName"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityOperationResult"></param>
        /// <returns>boolean true/false</returns>
        public static Boolean EvaluateActivityExitCriteria(Entity entity, WorkflowActionContext actionContext, Int64 workflowVersion, String activityName, CallerContext callerContext, EntityOperationResult entityOperationResult)
        {
            var exitActivity = true;

            //EntityFamilyQueue entityFamilyQueueItem = GetEntityFamilyQueueWithWorkflowChangeContext(entity.Id, actionContext, workflowVersion, activityName, String.Empty, EntityActivityList.UnKnown, "BusinessRuleProcessor.EvaluateActivityExitCriteria");

            //EntityOperationResultCollection entityOperationResultcollection = WorkflowHelper.EvaluateWorkflowBusinessRules(entityFamilyQueueItem, new EntityCollection() { entity }, MDMEvent.WorkflowActivityExitCriteria, callerContext);

            //if ((entityOperationResultcollection != null) && entityOperationResultcollection.Any())
            //{
            //    EntityOperationResult result = (EntityOperationResult)entityOperationResultcollection.GetByEntityId(entity.Id);

            //    if (result != null)
            //    {
            //        if (result.HasError)
            //        {
            //            exitActivity = false;
            //        }

            //        entityOperationResult.Errors.AddRange(result.Errors);
            //        entityOperationResult.Informations.AddRange(result.Informations);
            //        entityOperationResult.Warnings.AddRange(result.Warnings);
            //        entityOperationResult.RefreshOperationResultStatus();

            //        if (result.PassedBusinessConditionIdList != null)
            //        {
            //            foreach (Int32 i in result.PassedBusinessConditionIdList)
            //            {
            //                entityOperationResult.PassedBusinessConditionIdList.Add(i);
            //            }
            //        }

            //        if (result.FailedBusinessConditionIdList != null)
            //        {
            //            foreach (Int32 i in result.FailedBusinessConditionIdList)
            //            {
            //                entityOperationResult.FailedBusinessConditionIdList.Add(i);
            //            }
            //        }
            //    }
            //}

            return exitActivity;
        }

		/// <summary>
		/// Performs the merge for an entity and stores the merge item results. 
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="callerContext"></param>
		/// <returns></returns>
		public static Boolean ProcessMatchReview(Int64 entityId, CallerContext callerContext)
		{
			DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
			TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
            try
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Start();
                }

                #region Get Match Results

                IMatchingManager matchingManager = ServiceLocator.Current.GetInstance(typeof(IMatchingManager)) as IMatchingManager;

                if (matchingManager == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Service Locator returned a null instance of matching manager.");
                    }

                    return false;
                }

                IMatchingResult matchResult = matchingManager.GetMatchingResult(entityId, (Int64?)null);
                
                MatchingProfile matchProfile = matchingManager.GetMatchingProfile(matchResult.ProfileId);

                if (matchProfile == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Unable to find the match profile.");
                    }

                    return false;
                }
                
                #endregion
                
                #region Get the MergePlanItem from the match results and MergePlanning Profile
                IMatchReviewProfileManager mergePlanningProfileManager = ServiceLocator.Current.GetInstance(typeof(IMatchReviewProfileManager)) as IMatchReviewProfileManager;

                if (mergePlanningProfileManager == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Service Locator returned a null instance of mergeplanning profile manager.");
                    }

                    return false;
                }

                MatchReviewProfile mergePlanningProfile = mergePlanningProfileManager.GetById(matchProfile.MatchReviewProfileId, callerContext);
                IMergePlanningManager mergePlanningManager = ServiceLocator.Current.GetInstance(typeof(IMergePlanningManager)) as IMergePlanningManager;

                if (mergePlanningManager == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Service Locator returned a null instance of merge planning manager.");
                    }

                    return false;
                }

                MergePlanItem mergePlanItem = mergePlanningManager.BuildMergePlanItem(matchResult as MatchingResult, mergePlanningProfile, callerContext);
                #endregion

                if (mergePlanItem == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Call to BuildMergePlanItem returned a null mergeplanitem.");
                    }
                    return false;
                }

                #region Persist the MergePlanItem for the entity

                IMergePlanItemManager mergePlanItemManager = ServiceLocator.Current.GetInstance(typeof(IMergePlanItemManager)) as IMergePlanItemManager;

                if (mergePlanItemManager == null)
                {
                    if (currentTraceSettings.IsBasicTracingEnabled)
                    {
                        diagnosticActivity.LogWarning("ProcessMatchReview: Service Locator returned a null instance of merge plan item manager.");
                    }

                    return false;
                }


                OperationResult result = mergePlanItemManager.Create(mergePlanItem, callerContext);

                if (result.OperationResultStatus == OperationResultStatusEnum.Failed)
                {
                    return false;
                }

                #endregion

            }
            catch (Exception ex)
            {
                if (!currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity = new DiagnosticActivity(new ExecutionContext(MDMTraceSource.AdvancedWorkflow));
                    diagnosticActivity.Start();
                }

                diagnosticActivity.LogError(ex.Message);

                throw;
            }
            finally
            {
                if (currentTraceSettings.IsBasicTracingEnabled)
                {
                    diagnosticActivity.Stop();
                }
            }

			return true;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="callerContext"></param>
		/// <returns></returns>
		public static Boolean ExecuteMatchingReviewDecision(Int64 entityId, CallerContext callerContext)
	    {
			Boolean reviewComplete = false;

			DiagnosticActivity diagnosticActivity = new DiagnosticActivity();
			TraceSettings currentTraceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
			try
			{
				if (currentTraceSettings.IsBasicTracingEnabled)
				{
					diagnosticActivity.Start();
				}

				#region Get the MatchPlanItem Associated with an entity.

				IMergePlanItemManager mergePlanItemManager = ServiceLocator.Current.GetInstance(typeof (IMergePlanItemManager)) as IMergePlanItemManager;

				if (mergePlanItemManager == null)
				{
					if (currentTraceSettings.IsBasicTracingEnabled)
					{
						diagnosticActivity.LogWarning("ExecuteMatchingReviewDecision: Service Locator returned a null instance of merge plan item manager.");
					}

					return false;
				}

				MergePlanItemCollection mergePlanItemColl = mergePlanItemManager.GetByJobForSourceEntities(null, null, new System.Collections.ObjectModel.Collection<long>() {entityId}, callerContext);

				if (mergePlanItemColl == null)
				{
					if (currentTraceSettings.IsBasicTracingEnabled)
					{
						diagnosticActivity.LogWarning("ExecuteMatchingReviewDecision: Service Locator returned a null instance of merge plan item collection.");
					}

					return false;
				}

				MergePlanItem mergePlanItem = mergePlanItemColl.FirstOrDefault();

				if (mergePlanItem == null)
				{
					if (currentTraceSettings.IsBasicTracingEnabled)
					{
						diagnosticActivity.LogWarning("ExecuteMatchingReviewDecision: Merge plan item collection is empty.");
					}

					return false;
				}

				//Send the review status and uncomplete for user to take action.
				if (mergePlanItem.MergeActions.Contains(MergeAction.NeedsManualReview) || mergePlanItem.MergeActions.Contains(MergeAction.Unknown))
				{
					return false;
				}

				IMergingManager mergingManager = ServiceLocator.Current.GetInstance(typeof(IMergingManager)) as IMergingManager;

				if (mergingManager == null)
				{
					if (currentTraceSettings.IsBasicTracingEnabled)
					{
						diagnosticActivity.LogWarning("ExecuteMatchingReviewDecision: Service Locator returned a null instance of merging manager.");
					}

					return false;
				}

				MergeOperationResult operationResult = mergingManager.ProcessMergePlanItem(mergePlanItem, MergeMode.MergeExisting, null, callerContext);

				reviewComplete = (mergePlanItem.UserReviewStatus == MergePlanUserReviewStatus.NotRequired || mergePlanItem.UserReviewStatus == MergePlanUserReviewStatus.Complete);

				#endregion

				#region Execute Merge

				#endregion
			}
			catch(Exception ex)
		    {
				if (!currentTraceSettings.IsBasicTracingEnabled) 
				{
					diagnosticActivity = new DiagnosticActivity(new ExecutionContext(MDMTraceSource.AdvancedWorkflow));
					diagnosticActivity.Start();
				}

			    diagnosticActivity.LogError(ex.Message);

			    throw;
			}
			finally
			{
				if (currentTraceSettings.IsBasicTracingEnabled)
				{
					diagnosticActivity.Stop();
				}
			}

		    return reviewComplete;
	    }

	    #region Private Methods

			/// <summary>
			/// Creates a processor of the specified type.  Attempts to create the processor first
			/// from a constructor that accepts the rule xml document.  If the rule xml constructor 
			/// does not exist, the default parameter less constructor is used.
			/// Implementers can override this method to introduce constructors with alternative parameters.
			/// </summary>
			/// <param name="type">The type of processor to be created</param>
			/// <returns>An instance of the business rule processor</returns>
		private IBusinessRuleContext<IWorkflowBusinessRuleContext> GetProcessor(Type type)
        {
            IBusinessRuleContext<IWorkflowBusinessRuleContext> processor;
            var constructor = type.GetConstructor(new[] { typeof(XmlDocument) });
            if (constructor != null)
            {
                processor = (IBusinessRuleContext<IWorkflowBusinessRuleContext>)constructor.Invoke(new[] { RuleXmlDocument });
            }
            else
            {
                constructor = type.GetConstructor(Type.EmptyTypes);
                processor = (IBusinessRuleContext<IWorkflowBusinessRuleContext>)constructor.Invoke(null);
            }
            return processor;
        }


        private static EntityFamilyChangeContext CreateAsyncContext(WorkflowActionContext actionContext, Int64 workflowVersion, String activityName, String workflowRuntimeInstanceId)
        {
            EntityFamilyChangeContext entityFamilyChangeContext = new EntityFamilyChangeContext();
            WorkflowChangeContext workflowChangeContext = new MDM.BusinessObjects.WorkflowChangeContext();
            workflowChangeContext.ActivityName = activityName;
            workflowChangeContext.WorkflowRuntimeInstanceId = workflowRuntimeInstanceId;
            workflowChangeContext.WorkflowVersion = workflowVersion;
            workflowChangeContext.WorkflowActionContext = actionContext;
            entityFamilyChangeContext.WorkflowChangeContext = workflowChangeContext;
            return entityFamilyChangeContext;
        }

        private static EntityFamilyQueue GetEntityFamilyQueueWithWorkflowChangeContext(Int64 entityId, WorkflowActionContext actionContext, Int64 workflowVersion, String activityName, String workflowRuntimeInstanceId, EntityActivityList activityList, string programName)
        {
            EntityFamilyQueue entityFamilyQueue = new EntityFamilyQueue
            {
                EntityFamilyId = entityId,
                EntityGlobalFamilyId = entityId,
                IsProcessed = false,
                ProgramName = programName,
                EntityActivityList = activityList,
                Action = ObjectAction.Create
            };

            if (actionContext != null)
            {
                EntityFamilyChangeContext changeContext = CreateAsyncContext(actionContext, workflowVersion, activityName, workflowRuntimeInstanceId);
                if (changeContext != null)
                {
                    changeContext.EntityActivityList = activityList;
                    changeContext.EntityFamilyId = entityId;
                    changeContext.EntityGlobalFamilyId = entityId;
                    entityFamilyQueue.EntityFamilyChangeContexts.Add(changeContext);
                }
            }

            return entityFamilyQueue;
        }

        #endregion

        #endregion
    }
}