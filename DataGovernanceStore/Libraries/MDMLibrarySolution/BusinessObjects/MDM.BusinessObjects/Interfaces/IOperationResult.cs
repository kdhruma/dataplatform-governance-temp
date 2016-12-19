using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get operation result related information.
    /// </summary>
    public interface IOperationResult
    {
        #region Properties

        /// <summary>
        /// Indicates Check if there is any error in current operation result
        /// </summary>
        Boolean HasError { get; }

        /// <summary>
        /// Indicates whether there are any Information messages
        /// </summary>
        Boolean HasInformation
        {
            get;
        }

        /// <summary>
        /// Indicates whether there are any warning messages
        /// </summary>
        Boolean HasWarnings
        {
            get;
        }

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        /// <summary>
        /// Determines Id of operation Result
        /// </summary>
        Int32 Id { get; set; }

        /// <summary>
        /// Indicates the Reference Id
        /// </summary>
        String ReferenceId { get; set; }

        /// <summary>
        /// Indicates the action that was perfomed in this operation.
        /// </summary>
        ObjectAction PerformedAction { get; set; }

        /// <summary>
        /// Indicates Extended Properties from the Operation Xml file
        /// </summary>
        Hashtable ExtendedProperties { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of Operation Result
        /// </summary>
        /// <returns>Xml representation of Operation Result object</returns>
        String ToXml();

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, Collection<Object> parameters, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, OperationResultType operationResultType);

        /// <summary>
        /// Add operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="parameters">Additional Parameters that requires for OperationResult</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, IList<Object> parameters, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule id.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to ignore the error or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>       
        Boolean AddOperationResult(String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates additional parameters that requires for OperationResult</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to ignore the error or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>       
        Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds return value
        /// </summary>
        /// <param name="returnValue">Return value object</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddReturnValue(Object returnValue);

        /// <summary>
        /// Copies Errors and information from operation operation result to current
        /// </summary>
        /// <param name="operationResult">OperationResult from which errors and information is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors and information is to be copied is null</exception>
        void CopyErrorAndInfo(IOperationResult operationResult);

        /// <summary>
        /// Copies Errors, information and Warnings from operation operation result to current
        /// </summary>
        /// <param name="operationResult">OperationResult from which errors, information and Warning is to be copied</param>
        /// <exception cref="ArgumentNullException">Thrown if operationResult from which errors, information and Warning is to be copied is null</exception>
        void CopyErrorInfoAndWarning(IOperationResult operationResult);

        /// <summary>
        /// Gets information of Operation Result
        /// </summary>
        /// <returns>Collection of information</returns>
        Collection<IInformation> GetInformation();

        /// <summary>
        /// Gets errors
        /// </summary>
        /// <returns>Collection of errors</returns>
        Collection<IError> GetErrors();

        /// <summary>
        /// Gets warnings
        /// </summary>
        /// <returns>Collection of warnings</returns>
        IWarningCollection GetWarnings();

        #endregion
    }
}
