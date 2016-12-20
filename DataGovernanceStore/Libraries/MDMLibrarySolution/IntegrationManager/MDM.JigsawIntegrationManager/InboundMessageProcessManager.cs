using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDM.Core.Extensions;

namespace MDM.JigsawIntegrationManager
{
    using MDM.Core;
    using MDM.BusinessObjects;
    //using MDM.JigsawIntegrationManager.JigsawHelpers.OperationResultHelpers;
    using MDM.Utility;
    using BusinessObjects.Diagnostics;
    using MDM.Interfaces;
    using MDM.BusinessObjects.DQM;
    //using MDM.JigsawIntegrationManager.JigsawHelpers;

    public class InboundMessageProcessManager
    {
        #region Fields

        private IEntityManager _entityManager;
        private IMatchingResultManager _matchingResultManager;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public InboundMessageProcessManager(IEntityManager entityManager, IMatchingResultManager matchingResultManager)
        {
            _entityManager = entityManager;
            _matchingResultManager = matchingResultManager;
        }

        #endregion

        #region Methods

        //public EntityOperationResult Process(JigsawEntityOperationResult jigsawOperationResult, EntityProcessingOptions entityProcessingOptions, CallerContext callerContext)
        //{
        //    DiagnosticActivity diagnosticActivity = null;

        //    var traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings().Clone();

        //    var entityOperationResult = new EntityOperationResult();

        //    entityOperationResult.OperationResultStatus = Core.OperationResultStatusEnum.Pending;

        //    try
        //    {
        //        if (traceSettings.IsBasicTracingEnabled)
        //        {
        //            diagnosticActivity = new DiagnosticActivity();
        //            diagnosticActivity.Start();
        //        }

        //        JigsawOperationResultResponseStatus responseStatus;
        //        var response = jigsawOperationResult.EntityOperationResponse;

        //        #region Input Validations

        //        if (response == null || !Enum.TryParse(response.Status, out responseStatus))
        //        {
        //            String errorMessage = String.Format("Invalid parameter - jigsawOperationResult.EntityOperationResponse or Status {0}", response == null ? "Null Response" : response.Status);

        //            if (!traceSettings.IsBasicTracingEnabled)
        //            {
        //                diagnosticActivity = new DiagnosticActivity();
        //            }

        //            diagnosticActivity.LogError(errorMessage);
        //            diagnosticActivity.Stop();

        //            entityOperationResult.AddOperationResult("", errorMessage, OperationResultType.Error);

        //            return entityOperationResult;
        //        }

        //        if (_entityManager == null)
        //        {
        //            String errorMessage = String.Format("Entity Manager is NULL. Can't Process.");

        //            if (!traceSettings.IsBasicTracingEnabled)
        //            {
        //                diagnosticActivity = new DiagnosticActivity();
        //            }

        //            diagnosticActivity.LogError(errorMessage);
        //            diagnosticActivity.Stop();

        //            entityOperationResult.AddOperationResult("", errorMessage, OperationResultType.Error);

        //            return entityOperationResult;
        //        }

        //        #endregion

        //        var entities = response.ExtendedEntities.GetEntities();

        //        // TODO PRASAD HERE
        //        EntityOperationResultCollection entityOperationResults = null;

        //        var currentAction = responseStatus == JigsawOperationResultResponseStatus.@new ? ObjectAction.Create : ObjectAction.Update;

        //        foreach (var entity in entities)
        //        {
        //            entity.Action = currentAction;
        //        }

        //        if (responseStatus == JigsawOperationResultResponseStatus.@new ||
        //            responseStatus == JigsawOperationResultResponseStatus.existing)
        //        {
        //            // Status is new
        //            entityOperationResults = _entityManager.Process(entities, entityProcessingOptions, callerContext);
        //        }
        //        else if (responseStatus == JigsawOperationResultResponseStatus.maybe)
        //        {
        //            if (!FillEntityIds(response.ExtendedEntities, callerContext))
        //            {
        //                String errorMessage = "Some entity ids can't be filled due to the lack of existing entities with the same Guids.";
        //                entityOperationResult.AddOperationResult("", errorMessage, OperationResultType.Error);
        //                diagnosticActivity.LogError(errorMessage);

        //                return entityOperationResult;
        //            }

        //            var matchingResult = response.GetMatchingResult();
        //            EntityProcessLifeCycleDuplicate(matchingResult); // TODO: As discussed, just call an empty method for now corresponding to the flow diagram
        //            _matchingResultBL.Process(GetMatchingProfile(), new MatchingResultCollection { matchingResult }, callerContext);
        //        }
        //    }
        //    finally
        //    {
        //        if (traceSettings.IsBasicTracingEnabled)
        //        {
        //            diagnosticActivity.Stop();
        //        }
        //    }

        //    return entityOperationResult;
        //}

        // TODO: add some logic, do nothing for now
        
        private void EntityProcessLifeCycleDuplicate(MatchingResult matchResult)
        {
        }

        //private Boolean FillEntityIds(JigsawExtendedEntityCollection entities, CallerContext callerContext)
        //{
        //    Collection<Guid> entityGuids = entities.Select(e => new Guid(e.Eid)).ToCollection();
        //    Collection<Int64> entityIds = _entityManager.GetEntityGuidsMap(entityGuids, callerContext);

        //    if (entityIds.Count != entityGuids.Count)
        //    {
        //        return false;
        //    }

        //    for (int i = 0; i < entities.Count; i++)
        //    {
        //        entities[i].AttributesInfo.ExternalId = entityIds[i];
        //    }

        //    return true;
        //}

        private MatchingProfile GetMatchingProfile()
        {
            return new MatchingProfile
            {
                Id = 6,
                Name = "Profile1"
            };
        }


        #endregion
    }
}
