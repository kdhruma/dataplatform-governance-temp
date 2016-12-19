using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get a job.
    /// </summary>
    public interface IJob : IMDMObject
    {
        #region Properties

        /// <summary>
        ///  Property denoting the description of the Job
        /// </summary>
        String Description { get; set; }

        /// <summary>
        ///  Property denoting the id of the Job profile.
        /// </summary>
        Int32 ProfileId { get; set; }

        /// <summary>
        ///  Property denoting the Name of the Job profile.
        /// </summary>
        String ProfileName { get; set; }

        /// <summary>
        ///  Property denoting the computer name which has created job
        /// </summary>
        String ComputerName { get; set; }

        /// <summary>
        ///  Property denoting the create date of the Job
        /// </summary>
        String CreatedUser { get; set; }

        /// <summary>
        ///  Property denoting the created date of the Job
        /// </summary>
        String CreatedDateTime { get; set; }

        /// <summary>
        ///  Property denoting the modified date of the Job
        /// </summary>
        String ModifiedDateTime { get; set; }

        /// <summary>
        ///  Property denoting the Next Run date of the Job schedule
        /// </summary>
        String NextRunDate { get; set; }

        /// <summary>
        ///  Property denoting the Content Data.
        /// </summary>
        String JobDataXml { get; set; }

        /// <summary>
        ///  Property denoting the whether it is enable or not
        /// </summary>
        Boolean IsEnable { get; set; }

        /// <summary>
        ///  Property denoting the Type of the Job
        /// </summary>
        JobType JobType { get; set; }

        /// <summary>
        ///  Property denoting the sub type of the Job
        /// </summary>
        JobSubType JobSubType { get; set; }

        /// <summary>
        ///  Property denoting job action
        /// </summary>
        JobAction JobAction { get; set; }

        /// <summary>
        ///  Property denoting job status
        /// </summary>
        JobStatus JobStatus { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clone job object
        /// </summary>
        /// <returns>Cloned copy of job object.</returns>
        IJob Clone();

        /// <summary>
        /// Gets job data
        /// </summary>
        /// <returns>Job Data object</returns>
        /// <exception cref="NullReferenceException">Thrown when JobData is null</exception>
        IJobData GetJobData();

        /// <summary>
        /// Sets job data
        /// </summary>
        /// <param name="iJobData">Job data object</param>
        /// <returns>Result of the operation</returns>
        Boolean SetJobData(IJobData iJobData);

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <param name="serialization">Type of serialzation</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
