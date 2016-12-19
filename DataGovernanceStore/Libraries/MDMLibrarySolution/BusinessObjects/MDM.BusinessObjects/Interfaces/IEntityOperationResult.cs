using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity operation result.
    /// </summary>
    public interface IEntityOperationResult : IOperationResult
    {
        #region Properties

        /// <summary>
        /// Property denoting the id of the entity for which results are created
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting the long name of the entity for which results are created
        /// </summary>
        String EntityLongName { get; set; }

        /// <summary>
        /// Property denoting the reference id of the entity for which results are created
        /// </summary>
        new Int64 ReferenceId { get; set; }

        /// <summary>
        /// Property contains DataQualityIndicator values collection
        /// </summary>
        Collection<NamedDataQualityIndicatorValue> NamedDataQualityIndicatorValues { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Operation Result
        /// </summary>
        /// <returns>Xml representation of Entity Operation Result object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of Entity operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity operation result</returns>
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
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipId">Indicates Id of the relationship</param>
        /// <param name="resultCode">Indicates Result Code</param>
        /// <param name="resultMessage">Indicates Result Message</param>
        /// <param name="operationResultType">Indicates The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddRelationshipOperationResult(Int32 relationshipId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds relationship operation result
        /// </summary>
        /// <param name="relationshipTypeName">Indicates Type Name of the relationship for which result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates message format parameters</param>
        /// <param name="reasonType">Indicates reason type</param>
        /// <param name="ruleMapContextId">Indicates rule map context id</param>
        /// <param name="ruleId">Indicates rule id</param>
        /// <param name="operationResultType">Indicates The type of the result which needs to be added</param>
        /// <param name="ignoreError">Indicates whether to show the errors on UI or not. Default value is false</param>
        /// <returns>Returns Boolean result saying whether add is successful or not</returns>
        Boolean AddRelationshipOperationResult(String relationshipTypeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// Adds relationship attribute operation result
        /// </summary>
        /// <param name="relationshipId">Id of the relationship having attribute</param>
        /// <param name="attributeId">Id of the attribute for which result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>Boolean result saying whether add is successful or not</returns>
        Boolean AddRelationshipAttributeOperationResult(Int32 relationshipId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Adds relationship attribute operation result
        /// </summary>
        /// <param name="relationshipTypeName">Indicates name of the relationship type having attribute</param>
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
        Boolean AddRelationshipAttributeOperationResult(String relationshipTypeName, String attributeName, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType, Boolean ignoreError = false);

        /// <summary>
        /// This method copies entire EntityOperationResult
        /// </summary>
        /// <param name="entityOperationResult">source entity operation result</param>
        /// <param name="copyEntityMetadata">Boolean flag which indicates if copying entity metadata is required or not.</param>
        void CopyEntityOperationResult(IEntityOperationResult entityOperationResult, Boolean copyEntityMetadata);

        /// <summary>
        /// Get collection of AttributeOperation result. Which contains errors and warnings
        /// </summary>
        /// <returns>IAttributeOperationResultCollection</returns>
        IAttributeOperationResultCollection GetAttributeOperationResultCollection();

        /// <summary>
        ///  Get collection of Relationship Operation result. Which contains errors and warnings
        /// </summary>
        /// <returns>IRelationshipOperationResultCollection</returns>
        IRelationshipOperationResultCollection GetRelationshipOperationResultCollection();

        #endregion
    }
}
