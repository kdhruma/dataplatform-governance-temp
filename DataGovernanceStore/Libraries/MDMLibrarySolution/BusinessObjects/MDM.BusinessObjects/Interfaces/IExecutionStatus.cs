using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the execution status.
    /// </summary>
    public interface IExecutionStatus
    {
        #region Properties

        /// <summary>
        /// Property denoting start time of job
        /// </summary>
        String StartTime {get; set;}

        /// <summary>
        /// Property denoting end time of job
        /// </summary>
        String EndTime {get; set;}

        /// <summary>
        /// Property denoting the estimated milliseconds to complete job
        /// </summary>
        Double EstimatedMilliSeconds { get; set; }

        /// <summary>
        /// Property denoting the remaining milliseconds to complete job
        /// </summary>
        Double RemainingMilliSeconds { get; set; }

        /// <summary>
        /// Property denoting the total milliseconds to complete job
        /// </summary>
        Double TotalMilliSeconds {get; set;}
        
        /// <summary>
        /// Property denoting total elements to be processed by Job
        /// </summary>
        Int64 TotalElementsToProcess {get; set;}

        /// <summary>
        /// Property denoting current execution status of job in summarized form
        /// </summary>
        String CurrentStatusMessage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <param name="serialization">Type of ObjectSerialication</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
