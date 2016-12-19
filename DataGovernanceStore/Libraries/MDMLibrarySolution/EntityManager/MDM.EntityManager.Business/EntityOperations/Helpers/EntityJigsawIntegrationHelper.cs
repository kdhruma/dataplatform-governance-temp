using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using AttributeModelManager.Business;
    using BusinessObjects;
    using Core;
    using EntityModelManager.Business;
    using Interfaces;
    using JigsawIntegrationManager;
    using JigsawIntegrationManager.DataPackages;
    using MessageBrokerManager;
    using System.Collections.Generic;
    using System.Linq;
    using Utility;

    /// <summary>
    /// Class provides utility methods for manipulating entity context object
    /// </summary>
    internal class EntityJigsawIntegrationHelper
    {
        /// <summary>
        /// Helper method to get entities' current state, business conditions, score information and send to Jigsaw.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="mdmRuleParams"></param>
        /// <param name="securityPrincipal"></param>
        /// <param name="callerContext"></param>
        public  static void SendToJigsaw(EntityCollection entities, MDMRuleParams mdmRuleParams, SecurityPrincipal securityPrincipal, CallerContext callerContext)
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            var entityIdList = entities.GetEntityIdList();

            var stateValidationBL = new EntityStateValidationBL();

            var allValidationStates = stateValidationBL.Get(entityIdList, callerContext);
            var allScores = stateValidationBL.GetEntityStateValidationScores(entityIdList, callerContext);

            var allBusinessConditions = stateValidationBL.GetEntityBusinessConditions(entityIdList, callerContext);

            var entityMessageDataPackages = new List<EntityMessageDataPackage>();

            foreach (Entity entity in entities)
            {
                if (entity.Action == ObjectAction.Read || entity.Action == ObjectAction.Ignore)
                {
                    continue;
                }

                var entityMessageDataPackage = new EntityMessageDataPackage { Entity = entity };

                entityMessageDataPackage.StateValidations = new EntityStateValidationCollection(allValidationStates.Where(vs => vs.EntityId == entity.Id).ToList());
                entityMessageDataPackage.StateValidationScores = new EntityStateValidationScoreCollection(allScores.Where(score => score.EntityId == entity.Id).ToList());

                if (allBusinessConditions != null && allBusinessConditions.Count > 0)
                {
                    entityMessageDataPackage.BusinessCondition = allBusinessConditions.FirstOrDefault(bc => bc.EntityId == entity.Id);

                    if (entityMessageDataPackage.BusinessCondition != null && mdmRuleParams != null)
                    {
                        var qualifiedRuleMaps = mdmRuleParams.QualifiedRuleMaps;

                        if (qualifiedRuleMaps != null && qualifiedRuleMaps.Count > 0)
                        {
                            if (qualifiedRuleMaps.ContainsKey(entity.Id))
                            {
                                entityMessageDataPackage.RuleMaps = qualifiedRuleMaps[entity.Id];
                            }
                        }
                    }
                }

                entityMessageDataPackages.Add(entityMessageDataPackage);
            }

            MessageBrokerHelper.SendEntityMessage(entityMessageDataPackages, callerContext, JigsawCallerProcessType.DataQualityMessage, JigsawConstants.IntegrationBrokerType, securityPrincipal.CurrentUserName);
        }
    }
}
