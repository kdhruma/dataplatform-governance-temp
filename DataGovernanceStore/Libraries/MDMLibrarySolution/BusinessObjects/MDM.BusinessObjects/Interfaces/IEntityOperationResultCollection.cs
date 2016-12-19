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
    public interface IEntityOperationResultCollection : IEnumerable<EntityOperationResult>
    {
        #region Properties

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Operation Result Collection
        /// </summary>
        /// <returns>Xml representation of Entity Operation Result Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Operation Result Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Operation Result Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Adds entity operation result object in collection
        /// </summary>
        /// <param name="iEntityOperationResult">Indicates entity operation result to add in collection</param>
        void Add(IEntityOperationResult iEntityOperationResult);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType);
        
        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="parameters">Indicates the additional parameters that requires for operationResult</param>      
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType);

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        Boolean AddEntityOperationResult( Int64 entityId, String resultCode, String resultMessage, OperationResultType operationResultType );

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Indicates Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result message</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType);

        /// <summary>
        /// Adds entity operation result
        /// </summary>
        /// <param name="entityId">Indicates Id of the entity for which operation result needs to be added</param>
        /// <param name="resultCode">Indicates result code</param>
        /// <param name="resultMessage">Indicates result massage</param>  
        /// <param name="parameters">Indicates the additional parameters that requires for operationResult</param>      
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="operationResultType">Indicates type of the result which needs to be added</param>
        /// <returns>Returns boolean result saying whether add is successful or not</returns>
        Boolean AddEntityOperationResult(Int64 entityId, String resultCode, String resultMessage, Collection<Object> parameters, ReasonType reasonType, Int32 ruleMapContextId, Int32 ruleId, OperationResultType operationResultType);

        /// <summary>
        /// Adds entity attribute operation result
        /// </summary>
        /// <param name="entityId">Id of the entity for which operation result needs to be added</param>
        /// <param name="attributeId">Id of the entity attribute for which operation result needs to be added</param>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        Boolean AddAttributeOperationResult( Int64 entityId, Int32 attributeId, String resultCode, String resultMessage, OperationResultType operationResultType );

        /// <summary>
        /// Fetch EntityOperationResult by reference Id
        /// </summary>
        /// <param name="referenceId">Indicates the reference Id of an entity</param>
        /// <returns>EntityOperationResult having given referenceId</returns>
        IEntityOperationResult GetByReferenceId(Int64 referenceId);
      
        #endregion
    }
}
