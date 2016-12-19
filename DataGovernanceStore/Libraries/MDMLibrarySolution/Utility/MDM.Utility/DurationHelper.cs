using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Utility
{
    /// <summary>
    /// This is Utility class for Calculating timespan
    /// </summary>
    public class DurationHelper
    {
        private DateTime _originalStartTime;

        private DateTime _startTime;

        #region Properties

        /// <summary>
        /// Time when the durtion helper initialized
        /// </summary>
        public DateTime StartDateTime
        {
            get { return _originalStartTime; }
            set { _originalStartTime = value; }
        }

        #endregion

        /// <summary>
        /// Constructor with date time of a DurationHelper as input parameters
        /// </summary>
        /// <param name="startDateTime">Indicates start datetime</param>
        public DurationHelper(DateTime startDateTime)
        {
            _originalStartTime = _startTime = startDateTime;
        }

        /// <summary>
        /// returns the timespan in milliseconds,i.e difference between the current and previous call of this method
        /// </summary>
        /// <param name="endDateTime">end datetime</param>
        /// <returns>timespan in milliseconds</returns>
        public Double GetDurationInMilliseconds(DateTime endDateTime)
        {
            TimeSpan timeSpan = endDateTime - _startTime;
            _startTime = endDateTime;
            return timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Returns the cumulative timespan in milliseconds,i.e difference between the current and initial call of this method
        /// </summary>
        /// <returns>timespan in milliseconds</returns>
        public Double GetCumulativeTimeSpanInMilliseconds()
        {
            TimeSpan timeSpan = DateTime.Now - _originalStartTime;
            return timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetDuration()
        {
            _originalStartTime = DateTime.Now;
        }
    }
}
