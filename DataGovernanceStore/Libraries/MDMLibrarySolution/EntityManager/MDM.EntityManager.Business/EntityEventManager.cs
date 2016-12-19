using System;

namespace MDM.EntityManager.Business
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Provides entity event manager
    /// </summary>
    public sealed class EntityEventManager
    {
        private EntityEventManager()
        {
        }

        public static readonly EntityEventManager Instance = new EntityEventManager();

        public EventHandler<EntityEventArgs> EntityValidate;
        public EventHandler<EntityEventArgs> EntityAttributesCreating;
        public EventHandler<EntityEventArgs> EntityRelationShipsCreating;
        public EventHandler<EntityEventArgs> EntityHierarchyCreating;
        public EventHandler<EntityEventArgs> EntityExtensionsCreating;
        public EventHandler<EntityEventArgs> EntityCreating;
        public EventHandler<EntityEventArgs> EntityAttributesCreated;
        public EventHandler<EntityEventArgs> EntityRelationShipsCreated;
        public EventHandler<EntityEventArgs> EntityHierarchyCreated;
        public EventHandler<EntityEventArgs> EntityExtensionsCreated;
        public EventHandler<EntityEventArgs> EntityCreated;
        public EventHandler<EntityEventArgs> EntityReclassifying;
        public EventHandler<EntityEventArgs> EntityReclassified;
        public EventHandler<EntityEventArgs> EntityAttributesUpdating;
        public EventHandler<EntityEventArgs> EntityRelationShipsUpdating;
        public EventHandler<EntityEventArgs> EntityHierarchyUpdating;
        public EventHandler<EntityEventArgs> EntityExtensionsUpdating;
        public EventHandler<EntityEventArgs> EntityUpdating;
        public EventHandler<EntityEventArgs> EntityAttributesUpdated;
        public EventHandler<EntityEventArgs> EntityRelationShipsUpdated;
        public EventHandler<EntityEventArgs> EntityHierarchyUpdated;
        public EventHandler<EntityEventArgs> EntityExtensionsUpdated;
        public EventHandler<EntityEventArgs> EntityUpdated;
        public EventHandler<EntityEventArgs> EntityLoaded;
        public EventHandler<EntityEventArgs> EntityHierarchyChanged;
        public EventHandler<EntityEventArgs> EntityExtensionsChanged;
        public EventHandler<EntityEventArgs> EntityMerging;
        public EventHandler<EntityEventArgs> EntityMerged;
        public EventHandler<EntityEventArgs> DataQualityValidation;
        public EventHandler<EntityEventArgs> EntityNormalization;
        public EventHandler<EntityEventArgs> EntityCreatePostProcessStarting;
        public EventHandler<EntityEventArgs> EntityUpdatePostProcessStarting;
        public EventHandler<EntityEventArgs> EntityCreatePostProcessCompleted;
        public EventHandler<EntityEventArgs> EntityUpdatePostProcessCompleted;
        public EventHandler<EntityEventArgs> EntityDeletePostProcessStarting;
        public EventHandler<EntityEventArgs> EntityDeletePostProcessCompleted;
        public EventHandler<EntityEventArgs> EntityLoading;
        public EventHandler<EntityEventArgs> EntityFamilyPromoteQualifying;
        public EventHandler<EntityEventArgs> EntityFamilyPromoted;

        public void OnEntityValidate(EntityEventArgs e)
        {
            EntityValidate.SafeInvoke(this, e);
        }

        public void OnEntityAttributesCreating(EntityEventArgs e)
        {
            EntityAttributesCreating.SafeInvoke(this, e);
        }

        public void OnEntityRelationShipsCreating(EntityEventArgs e)
        {
            EntityRelationShipsCreating.SafeInvoke(this, e);
        }

        public void OnEntityHierarchyCreating(EntityEventArgs e)
        {
            EntityHierarchyCreating.SafeInvoke(this, e);
        }

        public void OnEntityExtensionsCreating(EntityEventArgs e)
        {
            EntityExtensionsCreating.SafeInvoke(this, e);
        }

        public void OnEntityCreating(EntityEventArgs e)
		{
			EntityCreating.SafeInvoke(this, e);
		}

        public void OnEntityAttributesCreated(EntityEventArgs e)
        {
            EntityAttributesCreated.SafeInvoke(this, e);
        }

        public void OnEntityRelationShipsCreated(EntityEventArgs e)
        {
            EntityRelationShipsCreated.SafeInvoke(this, e);
        }

        public void OnEntityHierarchyCreated(EntityEventArgs e)
        {
            EntityHierarchyCreated.SafeInvoke(this, e);
        }

        public void OnEntityExtensionsCreated(EntityEventArgs e)
        {
            EntityExtensionsCreated.SafeInvoke(this, e);
        }

        public void OnEntityCreated(EntityEventArgs e)
		{
			EntityCreated.SafeInvoke(this, e);
        }

        public void OnEntityReclassifying(EntityEventArgs e)
        {
            EntityReclassifying.SafeInvoke(this, e);
        }

        public void OnEntityReclassified(EntityEventArgs e)
        {
            EntityReclassified.SafeInvoke(this, e);
        }

        public void OnEntityAttributesUpdating(EntityEventArgs e)
        {
            EntityAttributesUpdating.SafeInvoke(this, e);
        }

        public void OnEntityRelationShipsUpdating(EntityEventArgs e)
        {
            EntityRelationShipsUpdating.SafeInvoke(this, e);
        }

        public void OnEntityHierarchyUpdating(EntityEventArgs e)
        {
            EntityHierarchyUpdating.SafeInvoke(this, e);
        }

        public void OnEntityExtensionsUpdating(EntityEventArgs e)
        {
            EntityExtensionsUpdating.SafeInvoke(this, e);
        }

        public void OnEntityUpdating(EntityEventArgs e)
        {
            EntityUpdating.SafeInvoke( this, e );
        }

        public void OnEntityAttributesUpdated(EntityEventArgs e)
        {
            EntityAttributesUpdated.SafeInvoke(this, e);
        }

        public void OnEntityRelationShipsUpdated(EntityEventArgs e)
        {
            EntityRelationShipsUpdated.SafeInvoke(this, e);
        }

        public void OnEntityHierarchyUpdated(EntityEventArgs e)
        {
            EntityHierarchyUpdated.SafeInvoke(this, e);
        }

        public void OnEntityExtensionsUpdated(EntityEventArgs e)
        {
            EntityExtensionsUpdated.SafeInvoke(this, e);
        }

        public void OnEntityUpdated(EntityEventArgs e)
        {
            EntityUpdated.SafeInvoke(this, e);
        }

        public void OnEntityLoaded(EntityEventArgs e)
        {
            EntityLoaded.SafeInvoke(this, e);
        }

        public void OnEntityHierarchyChanged(EntityEventArgs e)
        {
            EntityHierarchyChanged.SafeInvoke(this, e);
        }

        public void OnEntityExtensionsChanged(EntityEventArgs e)
        {
            EntityExtensionsChanged.SafeInvoke(this, e);
        }

        public void OnEntityMerging(EntityEventArgs e)
        {
            EntityMerging.SafeInvoke(this, e);
        }

        public void OnEntityMerged(EntityEventArgs e)
        {
            EntityMerged.SafeInvoke(this, e);
        }
      
        public void OnEntityCreatePostProcessStarting(EntityEventArgs e)
        {
            EntityCreatePostProcessStarting.SafeInvoke(this, e);
        }

        public void OnEntityCreatePostProcessCompleted(EntityEventArgs e)
        {
            EntityCreatePostProcessCompleted.SafeInvoke(this, e);
        }

        public void OnEntityUpdatePostProcessStarting(EntityEventArgs e)
        {
            EntityUpdatePostProcessStarting.SafeInvoke(this, e);
        }

        public void OnEntityUpdatePostProcessCompleted(EntityEventArgs e)
        {
            EntityUpdatePostProcessCompleted.SafeInvoke(this, e);
        }

        public void OnEntityDeletePostProcessStarting(EntityEventArgs e)
        {
            EntityDeletePostProcessStarting.SafeInvoke(this,e);
        }

        public void OnEntityDeletePostProcessCompleted(EntityEventArgs e)
        {
            EntityDeletePostProcessCompleted.SafeInvoke(this,e);
        }

        public void OnDataQualityValidationEvent(EntityEventArgs e)
        {
            DataQualityValidation.SafeInvoke(this, e);
        }

		public void OnEntityLoading(EntityEventArgs e)
        {
            EntityLoading.SafeInvoke(this, e);
        }

        public void OnEntityNormalizationEvent(EntityEventArgs e)
        {
            EntityNormalization.SafeInvoke(this, e);
        }

        public void OnEntityFamilyPromoteQualifying(EntityEventArgs e)
        {
            EntityFamilyPromoteQualifying.SafeInvoke(this, e);
        }

        public void OnEntityFamilyPromoted(EntityEventArgs e)
        {
            EntityFamilyPromoted.SafeInvoke(this, e);
        }
    }
}
