using System;
using System.Diagnostics;

namespace MDM.Core
{
    /// <summary>
    /// ConsoleAutoStopWatch Class
    /// </summary>
    public class ConsoleAutoStopWatch : IDisposable
    {
        private readonly Stopwatch _stopWatch;
        private readonly string _actionName;

        /// <summary>
        /// ConsoleAutoStopWatch constructor having actionName
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        public ConsoleAutoStopWatch(string actionName)
        {
            _stopWatch = Stopwatch.StartNew();
            _actionName = actionName;
        }

        public void Dispose()
        {
            _stopWatch.Stop();
            TimeSpan ts = _stopWatch.Elapsed;

            string elapsedTime = String.Format("{0} - {1}", _actionName, ts.TotalMilliseconds);
            Console.WriteLine(elapsedTime);
        }
    }
}

