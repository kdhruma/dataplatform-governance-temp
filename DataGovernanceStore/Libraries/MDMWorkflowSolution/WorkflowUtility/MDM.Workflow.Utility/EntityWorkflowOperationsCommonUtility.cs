using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MDM.Workflow.Utility
{
    using BusinessObjects;
    using Core;
    using Interfaces;
    using Utility;

    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityWorkflowOperationsCommonUtility
    {
        #region Fields
        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventEntities"></param>
        /// <param name="entityOperationResults"></param>
        /// <param name="callContext"></param>
        /// <param name="eventSource"></param>
        /// <param name="entityManager"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Boolean FireEntityWorkflowEvent(EntityCollection eventEntities, EntityOperationResultCollection entityOperationResults, CallerContext callContext, EventSource eventSource, IEntityManager entityManager, Int32 userId)
        {
            Boolean result = true;

            MDMPublisher publisher = Utility.GetMDMPublisher(callContext.Application, callContext.Module);
            if (callContext.MDMPublisher == MDMPublisher.Unknown)
                callContext.MDMPublisher = publisher;

            callContext.MDMSource = eventSource;

            var eventArgs = new EntityWorkflowEventArgs(eventEntities, entityManager, entityOperationResults, userId, callContext);

            switch (eventSource)
            {
                case EventSource.EntityTransitioning:
                    EntityWorkflowEventManager.Instance.OnEntityTransitioning(eventArgs);
                    break;
                case EventSource.EntityTransitioned:
                    EntityWorkflowEventManager.Instance.OnEntityTransitioned(eventArgs);
                    break;
                case EventSource.EntityAssignmentChanging:
                    EntityWorkflowEventManager.Instance.OnEntityAssignmentChanging(eventArgs);
                    break;
                case EventSource.EntityAssignmentChanged:
                    EntityWorkflowEventManager.Instance.OnEntityAssignmentChanged(eventArgs);
                    break;
                default:
                    break;
            }

            if (entityOperationResults != null)
            {
                entityOperationResults.RefreshOperationResultStatus();
                result = ScanAndFilterEntities(eventEntities, entityOperationResults);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="entityORC"></param>
        /// <returns></returns>
        public static Boolean ScanAndFilterEntities(EntityCollection entities, EntityOperationResultCollection entityORC)
        {
            Boolean continueProcess = true;

            if (entities != null && entityORC != null)
            {
                IEnumerable<EntityOperationResult> erroredEntityORC = entityORC.Where(orc => orc.OperationResultStatus == OperationResultStatusEnum.CompletedWithErrors || orc.OperationResultStatus == OperationResultStatusEnum.Failed);

                if (erroredEntityORC != null && erroredEntityORC.Count() > 0)
                {
                    //Get EntityOR having status = completed with errors
                    foreach (EntityOperationResult entityOR in erroredEntityORC)
                    {
                        //Get entity from collection which got completed with error
                        IEntity erroredEntity = entities.GetEntity(entityOR.EntityId);

                        //Errored entity is found. So remove it from entity collection
                        if (erroredEntity != null)
                        {
                            entities.Remove((Entity)erroredEntity);
                        }
                    }

                    //if all entities are errored out, no need to proceed further processing.
                    if (entities.Count < 1)
                    {
                        continueProcess = false;
                    }
                }
                else
                {
                    continueProcess = true;
                }
            }

            return continueProcess;
        }

        #endregion

        #endregion
    }
}
