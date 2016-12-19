using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of relationship operation results.
    /// </summary>
    public interface IRelationshipOperationResultCollection : IEnumerable<RelationshipOperationResult>
    {
        #region Properties

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship Operation Result Collection
        /// </summary>
        /// <returns>Xml representation of Relationship Operation Result Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship Operation Result Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship Operation Result Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

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
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates collection of parameters</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, Collection<Object> parameters, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType);

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipId">Id of the relationship for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipTypeName">Indicates name of the relationship type</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns result saying whether add is successful or not</returns>
        Boolean AddRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds relationship attribute operation result
        /// </summary>
        /// <param name="relationshipId">Id of the relationship for which operation result needs to be added</param>
        /// <param name="attributeId">Id of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds entity attribute operation result
        /// </summary>
        /// <param name="relationshipTypeName">Indicates name of the relationship type for which operation result needs to be added</param>
        /// <param name="attributeName">Indicates name of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        #endregion
    }
}
