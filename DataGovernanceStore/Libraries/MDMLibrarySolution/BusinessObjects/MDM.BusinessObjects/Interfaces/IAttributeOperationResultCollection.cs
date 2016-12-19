using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute operation result.
    /// </summary>
    public interface IAttributeOperationResultCollection : IEnumerable<AttributeOperationResult>
    {
        #region Properties

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute Operation Result Collection
        /// </summary>
        /// <returns>Xml representation of Attribute Operation Result Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute Operation Result Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Operation Result Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="attributeName">Indicates Id of the attribute for which operation result needs to be added</param>
        /// <param name="messageCode">Indicates message Code</param>
        /// <param name="parameters">Indicates message parameters</param>
        /// <param name="reasonType">Indicates result massage</param>
        /// <param name="ruleMapContextId">Indicates business rule map context id</param>
        /// <param name="ruleId">Indicates business rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(String attributeName, String messageCode, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="attributeName">Indicates name of the attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(String attributeName, String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        #endregion
    }
}
