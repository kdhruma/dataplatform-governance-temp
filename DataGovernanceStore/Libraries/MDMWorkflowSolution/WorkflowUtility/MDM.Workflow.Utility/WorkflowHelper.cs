using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceLocation;

namespace MDM.Workflow.Utility
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Utility;
    using MDM.Interfaces;

    /// <summary>
    /// Contains the helper methods required for workflow
    /// </summary>
    public class WorkflowHelper
    {
        #region Fields

        /// <summary>
        /// Field which indicates whether a new GUID needs to be generated whenever an MDMActivity has been instantiated
        /// Default value is set as 'false', asking to generate a new GUID everytime..
        /// Please do not change the default value!!!
        /// </summary>
        private static Boolean _generateNewGUIDForActivity = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Field which indicates whether a new GUID needs to be generated whenever an MDMActivity has been instantiated
        /// </summary>
        public static Boolean GenerateNewGUIDForActivity
        {
            get
            {
                return _generateNewGUIDForActivity;
            }
            set
            {
                _generateNewGUIDForActivity = value;
            }
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Get the bookmark name.
        /// Creates the bookmark name by merging Activity Name, InstanceId
        /// </summary>
        /// <param name="activityName">Current activity which is being executed</param>
        /// <param name="runtimeInstanceId">Runtime instance id for the currently running workflow instance id</param>
        /// <returns>Returns merged bookmark name</returns>
        public static String GetBookmarkName(String activityName, String runtimeInstanceId)
        {
            String returnValue = String.Empty;
            if (!String.IsNullOrEmpty(activityName) && !String.IsNullOrEmpty(runtimeInstanceId))
            {
                returnValue = String.Format("{0}_{1}", activityName, runtimeInstanceId);
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyQueueItem"></param>
        /// <param name="mdmdEvent"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static EntityOperationResultCollection EvaluateWorkflowBusinessRules(EntityFamilyQueue entityFamilyQueueItem, MDMEvent mdmdEvent, CallerContext callerContext)
        {
           //IEntityManager entityManager = ServiceLocator.Current.GetInstance(typeof(IEntityManager)) as IEntityManager;

           // EntityContext entityContext = new EntityContext()
           // {
           //     LoadAttributes = false,
           //     LoadEntityProperties = true,
           //     LoadStateValidationAttributes = true,
           //     LoadWorkflowInformation = true
           // };

           // Entity entity = entityManager.Get(entityFamilyQueueItem.EntityFamilyId, entityContext, MDMCenterApplication.PIM, MDMCenterModules.WindowsWorkflow);
           // EntityCollection entityCollection = new EntityCollection { entity };

           // EntityOperationResultCollection entityOperationResultCollection = EvaluateWorkflowBusinessRules(entityFamilyQueueItem, entityCollection, mdmdEvent, callerContext);

           // return entityOperationResultCollection;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityFamilyQueueItem"></param>
        /// <param name="entityCollection"></param>
        /// <param name="mdmdEvent"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public static EntityOperationResultCollection EvaluateWorkflowBusinessRules(EntityFamilyQueue entityFamilyQueueItem, EntityCollection entityCollection, MDMEvent mdmdEvent, CallerContext callerContext)
        {
            EntityOperationResultCollection entityOperationResultCollection = new EntityOperationResultCollection(entityCollection);

            EntityFamilyChangeContext familyChangeContext = entityFamilyQueueItem.EntityFamilyChangeContexts.GetByEntityFamilyId(entityFamilyQueueItem.EntityFamilyId);

            if ((familyChangeContext != null) && (familyChangeContext.WorkflowChangeContext != null))
            {
                MDMRuleParams mdmRuleParams = new MDMRuleParams()
                {
                    Entities = entityCollection,
                    EntityOperationResults = entityOperationResultCollection,
                    UserSecurityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal(),
                    CallerContext = callerContext,
                    Events = new Collection<MDMEvent>() { mdmdEvent },
                    WorkflowChangeContext = familyChangeContext.WorkflowChangeContext,
                    DDGCallerModule = DDGCallerModule.Workflow
                };

                if (entityCollection != null && entityCollection.Count > 0)
                {
                    //IEntityManager entityManager = ServiceLocator.Current.GetInstance(typeof(IEntityManager)) as IEntityManager;

                    //var preLoadEntityContext = PreLoadContextHelper.GetEntityContext(mdmRuleParams, entityManager);
                    //entityManager.EnsureEntityData(entityCollection, preLoadEntityContext, callerContext);

                    //MDMRuleEvaluator.Evaluate(mdmRuleParams);
                    //mdmRuleParams.EntityOperationResults.RefreshOperationResultStatus();
                }
            }

            return entityOperationResultCollection;
        }

        #endregion
    }
}