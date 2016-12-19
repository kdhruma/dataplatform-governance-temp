using System;

namespace Riversand.JobService
{
    public enum Type { JobRunner, JobScheduler, Configuration, RunCommand, CacheInvalidate }
}

namespace Riversand.JobService.Interfaces
{
    /// <summary>
    /// Summary description for IJobService.
	/// </summary>
	public interface IJobService
	{
        void OnNotifyChange(object pNotificationType);
        void OnTest();
        void OnNotifyConfigUpdate(object pNotificationType, String configName);
        void OnNotifyConfigReload(object pNotificationType);
        void QueueJobForRun(object pNotificationType, Int32 jobId);
        void OnNotifyCacheInvalidate(object pNotificationType, String cacheKey, String action);
	}
}
