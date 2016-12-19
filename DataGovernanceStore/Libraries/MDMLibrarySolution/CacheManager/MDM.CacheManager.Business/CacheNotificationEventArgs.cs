using System;

namespace MDM.CacheManager.Business
{
    /// <summary>
    /// Represents Event argument data for CacheNotifications
    /// </summary>
    public class CacheNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// Field specifying the cache key for which the cache notification event is triggered
        /// </summary>
        private String _cacheKey;

        /// <summary>
        /// Property specifying the cache key for which the cache notification event is triggered
        /// </summary>
        public String CacheKey
        {
            get { return _cacheKey; }
            set { _cacheKey = value; }
        }
    }
}
