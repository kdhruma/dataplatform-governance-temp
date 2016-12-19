using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;    

    /// <summary>
    /// Exposes methods or properties to set or get relationship operation result.
    /// </summary>
    public interface IRelationshipOperationResult : IOperationResult
    {
        #region Properties

        /// <summary>
        /// Property denoting the id of the relationship for which results are created
        /// </summary>
        Int64 RelationshipId { get; set; }

        /// <summary>
        /// Property denoting the reference id of the relationship for which results are created
        /// </summary>
        new Int64 ReferenceId { get; set; }

        /// <summary>
        /// Property denoting the external Id of the related entity
        /// </summary>
        String ToExternalId { get; set; }

        /// <summary>
        /// Property denoting the type of the relationship
        /// </summary>
        String RelationshipTypeName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Relationship Operation Result
        /// </summary>
        /// <returns>Xml representation of Relationship Operation Result object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of Relationship operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Relationship operation result</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Adds attribute operation result
        /// </summary>
        /// <param name="attributeId">Id of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds attribute operation result
        /// </summary>
        /// <param name="attributeName">Indicates name of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult(String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipId">Id of the relationships</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddChildRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result for child relationships
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
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddChildRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipId">Id of the relationships</param>
        /// <param name="attributeId">Id of the attribute</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddChildRelationshipAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result for child relationships
        /// </summary>
        /// <param name="relationshipTypeName">Indicates name of the relationship type</param>
        /// <param name="attributeName">Indicates name of the attribute</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns result saying whether add is successful or not</returns>
        Boolean AddChildRelationshipAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        #endregion
    }
}
