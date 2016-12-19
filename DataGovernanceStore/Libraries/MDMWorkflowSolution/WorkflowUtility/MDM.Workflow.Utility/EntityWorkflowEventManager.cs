using System;

namespace MDM.Workflow.Utility
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Provides entity event manager
    /// </summary>
    public sealed class EntityWorkflowEventManager
    {
        private EntityWorkflowEventManager()
        {
        }

        public static readonly EntityWorkflowEventManager Instance = new EntityWorkflowEventManager();

        public EventHandler<EntityWorkflowEventArgs> EntityTransitioning;
        public EventHandler<EntityWorkflowEventArgs> EntityTransitioned;
        public EventHandler<EntityWorkflowEventArgs> EntityAssignmentChanging;
        public EventHandler<EntityWorkflowEventArgs> EntityAssignmentChanged;
        public EventHandler<WorkItemsDataLoadedEventArgs> WorkItemsDataLoaded;

        public void OnEntityTransitioning(EntityWorkflowEventArgs e)
        {
            EntityTransitioning.SafeInvoke(this, e);
        }

        public void OnEntityTransitioned(EntityWorkflowEventArgs e)
        {
            EntityTransitioned.SafeInvoke(this, e);
        }

        public void OnEntityAssignmentChanging(EntityWorkflowEventArgs e)
        {
            EntityAssignmentChanging.SafeInvoke(this, e);
        }

        public void OnEntityAssignmentChanged(EntityWorkflowEventArgs e)
        {
            EntityAssignmentChanged.SafeInvoke(this, e);
        }

        public void OnWorkItemsDataLoaded(WorkItemsDataLoadedEventArgs e)
        {
            WorkItemsDataLoaded.SafeInvoke(this, e);
        }
    }
}
