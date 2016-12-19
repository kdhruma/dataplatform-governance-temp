using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get schedule criteria.
    /// </summary>
    public interface IScheduleCriteria : IMDMObject
    {
        /// <summary>
        /// Indicates DailyDelay
        /// </summary>
        Int32 DailyDelay { get; set; }

        /// <summary>
        /// Indicates Daily Frequency
        /// </summary>
        MDM.Core.DailyFrequencyOptions DailyFrequency { get; set; }

        /// <summary>
        /// Indicates day of month
        /// </summary>
        Int32 DayOfMonth { get; set; }

        /// <summary>
        /// Indicates day of week
        /// </summary>
        DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Indicates days of week
        /// </summary>
        System.Collections.ObjectModel.Collection<DayOfWeek> DaysOfWeek { get; set; }

        /// <summary>
        /// Indicates Effective End Date
        /// </summary>
        DateTime EffectiveEndDate { get; set; }

        /// <summary>
        /// Indicates Start Date
        /// </summary>
        DateTime EffectiveStartDate { get; set; }

        /// <summary>
        /// Indicates end time
        /// </summary>
        String EndTime { get; set; }

        /// <summary>
        /// Generate XMl
        /// </summary>
        /// <returns>XML of an Object</returns>
        String GenerateXml();

        /// <summary>
        /// Generate XMl for file
        /// </summary>
        /// <param name="filename">Name of the file</param>
        void GenerateXml(String filename);

        /// <summary>
        /// Get Next occurrence of Schedule
        /// </summary>
        /// <returns>Date and time</returns>
        DateTime GetNextOccurrence();

        /// <summary>
        /// Get Next occurrence of Schedule by given seed
        /// </summary>
        /// <param name="seed">DateTime</param>
        /// <returns>Date and time</returns>
        DateTime GetNextOccurrence(DateTime seed);

        /// <summary>
        /// Indicates monthly frequency
        /// </summary>
        MDM.Core.MonthlyFrequencyOptions MonthlyFrequency { get; set; }

        /// <summary>
        /// Indicates Recurrence Delay
        /// </summary>
        Int32 RecurrenceDelay { get; set; }

        /// <summary>
        /// Indicates Recurrence Frequency
        /// </summary>
        MDM.Core.RecurrenceFrequencyOptions RecurrenceFrequency { get; set; }

        /// <summary>
        /// Indicates Start Time
        /// </summary>
        String StartTime { get; set; }

        /// <summary>
        /// Indicate week of month
        /// </summary>
        Int32 WeekOfMonth { get; set; }
    }
}
