using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get business rule operation results.
    /// </summary>
    public interface IBusinessRuleOperationResultCollection : ICollection<BusinessRuleOperationResult>
    {
        #region Properties

        /// <summary>
        /// Returns the number of elements contained in Business rule operation result collection
        /// </summary>
        new Int32 Count { get; }

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Add business rule operation result object in collection
        /// </summary>
        /// <param name="businessRuleOperationResult">Indicates the business rule operation result</param>
        void Add(IBusinessRuleOperationResult businessRuleOperationResult);

        /// <summary>
        /// Add business rule operation results object in collection
        /// </summary>
        /// <param name="businessRuleOperationResults">Indicates the business rule operation result</param>
        void AddRange(IBusinessRuleOperationResultCollection businessRuleOperationResults);

        /// <summary>
        /// Get the business rule operationresult by reference id
        /// </summary>
        /// <param name="referenceId">Indicates the reference Id</param>
        /// <returns>Returns BusinessRuleOperationresult</returns>
        BusinessRuleOperationResult GetBusinessRuleOperationResultByReferenceId(Int64 referenceId);

        /// <summary>
        /// Get the business rule operation result by operation result status
        /// </summary>
        /// <param name="status">Indicates the OperationResultStatus</param>
        /// <returns>Returns BusinessRuleOperationresultCollection</returns>
        BusinessRuleOperationResultCollection GetBusinessRuleOperationResultByOperationResultStatus(OperationResultStatusEnum status);

        /// <summary>
        /// Returns errors
        /// </summary>
        /// <returns>Collection of error</returns>
        ErrorCollection GetErrors();

        /// <summary>
        /// Returns informations
        /// </summary>
        /// <returns>Collection of information</returns>
        InformationCollection GetInformations();

        /// <summary>
        /// Returns warnings
        /// </summary>
        /// <returns>Collection of warnings</returns>
        WarningCollection GetWarnings();

        /// <summary>
        /// Get Xml representation of Business rule operation result collection
        /// </summary>
        /// <returns>Xml representation of Business rule operation result collection object</returns>
        String ToXml();

        /// <summary>
        /// Get business rule operation results having errored results
        /// </summary>
        /// <returns>Returns business rule operation results having errored results</returns>
        BusinessRuleOperationResultCollection GetErroredBusinessRuleOperationResults();

        /// <summary>
        /// Calculates the status and updates with new status
        /// </summary>
        void RefreshBusinessRuleOperationResultStatus();

        #endregion
    }
}
