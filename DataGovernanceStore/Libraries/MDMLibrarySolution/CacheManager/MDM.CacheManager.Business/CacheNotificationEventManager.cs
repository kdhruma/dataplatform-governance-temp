using System;

namespace MDM.CacheManager.Business
{
    using MDM.Core;

    /// <summary>
    /// Represents an EventManager for managing cache notification events
    /// </summary>
    public sealed class CacheNotificationEventManager
    {
        /// <summary>
        /// Field which holds a reference to the static CacheNotificationEventManager instance
        /// </summary>
        public static readonly CacheNotificationEventManager Instance = new CacheNotificationEventManager();

        /// <summary>
        /// Field denoting the event handler for Cache update operations
        /// </summary>
        public EventHandler<CacheNotificationEventArgs> CacheUpdate;

        /// <summary>
        /// Field denoting the event handler for Cache removal operations
        /// </summary>
        public EventHandler<CacheNotificationEventArgs> CacheRemoval;
        
        #region Methods

        /// <summary>
        /// Triggers the cache update event
        /// </summary>
        /// <param name="eventArgs">Represents the event argument</param>
        public void OnCacheUpdate(CacheNotificationEventArgs eventArgs)
        {
            CacheUpdate.SafeInvoke(this, eventArgs);
        }

        /// <summary>
        /// Triggers the cache removal event
        /// </summary>
        /// <param name="eventArgs">Represents the event argument</param>
        public void OnCacheRemoval(CacheNotificationEventArgs eventArgs)
        {
            CacheRemoval.SafeInvoke(this, eventArgs);
        }

        #endregion Methods

    }
}
