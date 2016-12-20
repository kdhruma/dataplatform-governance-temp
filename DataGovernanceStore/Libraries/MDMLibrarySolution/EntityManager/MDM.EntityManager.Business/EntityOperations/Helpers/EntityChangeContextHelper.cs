using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.EntityManager.Business.EntityOperations.Helpers
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using BusinessObjects.Diagnostics;

    /// <summary>
    /// Class provides utility methods for calculating entity family change context object
    /// </summary>
    internal sealed class EntityChangeContextHelper
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets entity family change context  based on given entities
        /// </summary>
        /// <param name="entities">Indicates the collection of entity object for which change context to be calculated</param>
        /// <param name="callerContext">Specifies the caller context that describes the module and application that invoked the method</param>
        /// <param name="hasRelationshipsChanged">Indicates whether relationships are changed or not</param>
        /// <param name="traceSettings">Indicates trace setting to log diagnostic activity</param>
        /// <returns>Collection of entity family change context based on given entities</returns>
        public static EntityFamilyChangeContextCollection GetEntityFamilyChangeContexts(EntityCollection entities, CallerContext callerContext, Boolean hasRelationshipsChanged = false, TraceSettings traceSettings = null)
        {
            var diagnosticActivity = new DiagnosticActivity();

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Start();
            }

            EntityFamilyChangeContextCollection entityFamilyChangeContexts = null;

            if (entities != null && entities.Count > 0)
            {
                entityFamilyChangeContexts = new EntityFamilyChangeContextCollection();

                foreach (Entity entity in entities)
                {
                    //If entity action is set to read or ignore or entity type of category then skip those entities to log into change context.
                    //In case of denormalized relationships process, we may not have any entity level change. Hence allow process to refresh change contexts 
                    if ((entity.Action == ObjectAction.Read || entity.Action == ObjectAction.Ignore || entity.EntityTypeId == Constants.CATEGORY_ENTITYTYPE) && !hasRelationshipsChanged)
                    {
                        continue;
                    }

                    EntityChangeContext entityChangeContext = (EntityChangeContext)entity.GetChangeContext(true);
                    Int64 entityFamilyId = entity.EntityFamilyId;

                    EntityFamilyChangeContext entityFamilyChangeContext = entityFamilyChangeContexts.GetByEntityFamilyId(entityFamilyId);

                    if (entityFamilyChangeContext == null)
                    {
                        entityFamilyChangeContext = new EntityFamilyChangeContext(entityFamilyId, entity.EntityGlobalFamilyId, entity.OrganizationId, entity.ContainerId);
                        entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.Add(entityChangeContext);

                        entityFamilyChangeContexts.Add(entityFamilyChangeContext);
                    }
                    else
                    {
                        entityFamilyChangeContext.VariantsChangeContext.EntityChangeContexts.Add(entityChangeContext);
                    }
                }
            }

            if (traceSettings != null && traceSettings.IsBasicTracingEnabled)
            {
                diagnosticActivity.Stop();
            }

            return entityFamilyChangeContexts;
        }

        /// <summary>
        /// Refreshes entity family change context 
        /// </summary>
        /// <param name="entities">Indicates the collection of entity object for which change context to be calculated</param>
        /// <param name="entityFamilyChangeContext">Indicates delta family change context</param>
        /// <param name="callerContext">Specifies the caller context that describes the module and application that invoked the method</param>
        /// <param name="traceSettings">Indicates trace setting to log diagnostic activity</param>
        public static void RefreshEntityFamilyChangeContext(EntityCollection entities, EntityFamilyChangeContext entityFamilyChangeContext, CallerContext callerContext, TraceSettings traceSettings = null)
        {
            Boolean hasRelationshipsChanged = entityFamilyChangeContext.HasRelationshipsChanged();

            EntityFamilyChangeContextCollection deltaEntityFamilyChangeContexts = EntityChangeContextHelper.GetEntityFamilyChangeContexts(entities, callerContext, hasRelationshipsChanged, traceSettings);

            if (deltaEntityFamilyChangeContexts != null && deltaEntityFamilyChangeContexts.Count > 0)
            {
                entityFamilyChangeContext.Merge(deltaEntityFamilyChangeContexts.FirstOrDefault());
            }
        }

        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}