using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get job import result.
    /// </summary>
    public interface IJobImportResult
    {
        #region Properties

        /// <summary>
        ///  Property denoting the Job Id
        /// </summary>
        Int32 JobId { get; set; }

        /// <summary>
        ///  Property denoting the status of the job result
        /// </summary>
        String Status { get; set; }

        /// <summary>
        ///  Property denoting the external Id of the object type
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        ///  Property denoting the internal Id of the object type
        /// </summary>
        Int64 InternalId { get; set; }

        /// <summary>
        ///  Property denoting the description of the Job Import Error
        /// </summary>
        String Description { get; set; }

        /// <summary>
        ///  Property denoting the Audit Ref Id
        /// </summary>
        Int64 AuditRefId { get; set; }

        /// <summary>
        ///  Property denoting the operation result
        /// </summary>
        String OperationResultXML { get; set; }

        /// <summary>
        ///  Property denoting type of job
        /// </summary>
        ObjectType ObjectType { get; set; }

        /// <summary>
        ///  Property denoting the action performed on the object
        /// </summary>
        ObjectAction PerformedAction { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Job Import Result
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
