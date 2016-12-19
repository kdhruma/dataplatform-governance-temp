using System;
using System.Diagnostics;

namespace MDM.Core
{
    /// <summary>
    /// Exact time measurement for performance testing
    /// </summary>
    public sealed class BenchmarkTime : IDisposable
    {
        private readonly Stopwatch _stopWatch;
        private readonly String _actionName;
        private readonly Action<String, TimeSpan> _loggerAction;

        /// <summary>
        /// Start time measurement
        /// </summary>
        /// <param name="actionName">Name of action</param>
        /// <param name="loggerAction">Action for log result</param>
        public BenchmarkTime(String actionName, Action<String, TimeSpan> loggerAction)
        {
            _stopWatch = Stopwatch.StartNew();
            _actionName = actionName;
            _loggerAction = loggerAction;
        }

        /// <summary>
        /// Stop time measurement
        /// </summary>
        public void Dispose()
        {
            _stopWatch.Stop();
            TimeSpan ts = _stopWatch.Elapsed;
            if (_loggerAction != null)
            {
                _loggerAction(_actionName, ts);
            }
        }
    }
}

