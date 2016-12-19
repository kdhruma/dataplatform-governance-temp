using System;

namespace Riversand.JobService.Interfaces
{
    /// <summary>
    /// Summary description for IJob.
    /// </summary>
    public interface IJob
    {
        void Execute();
        void CleanUp();
        Boolean IsComplete();
        Boolean IsIgnored();
        Boolean cancelJob
        {
            get;
            set;
        }
        Int32 id
        {
            get;
        }
        String description
        {
            get;
            set;
        }
        String username
        {
            get;
        }
        void SaveJob();

    }
}
