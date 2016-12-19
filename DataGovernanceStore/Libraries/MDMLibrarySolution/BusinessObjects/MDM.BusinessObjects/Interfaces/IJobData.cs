using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get job data.
    /// </summary>
    public interface IJobData
    {
        #region Properties

        /// <summary>
        ///  Property denoting the profile id
        /// </summary>
        Int32 ProfileId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets execution status
        /// </summary>
        /// <returns>Execution Status object</returns>
        /// <exception cref="NullReferenceException">Thrown when Execution Status is NULL</exception>
        IExecutionStatus GetExecutionStatus();

        /// <summary>
        /// Sets execution status
        /// </summary>
        /// <param name="iExecutionStatus">The execution status object</param>
        /// <returns>The result of the operation</returns>
        Boolean SetExecutionStatus(IExecutionStatus iExecutionStatus);

        /// <summary>
        /// Gets job parameters
        /// </summary>
        /// <returns>Job Parameters</returns>
        /// <exception cref="NullReferenceException">Thrown when JobParameters field is NULL</exception>
        IJobParameterCollection GetJobParameters();

        /// <summary>
        /// Sets job parameters
        /// </summary>
        /// <param name="iJobParameterCollection">Job parameters collection</param>
        /// <returns>The result of the operation</returns>
        Boolean SetJobParameters(IJobParameterCollection iJobParameterCollection);

        /// <summary>
        /// Gets job operation result
        /// </summary>
        /// <returns>Job operation result</returns>
        /// <exception cref="NullReferenceException">Thrown when OperationResult field is NULL</exception>
        IOperationResult GetJobOperationResult();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <param name="serialization">Type of an Object Serialization</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
