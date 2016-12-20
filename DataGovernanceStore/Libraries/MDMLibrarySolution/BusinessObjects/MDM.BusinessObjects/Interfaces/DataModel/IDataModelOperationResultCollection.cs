using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects.DataModel;
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get attribute operation result.
    /// </summary>
    public interface IDataModelOperationResultCollection : IEnumerable<DataModelOperationResult>
    {
        #region Properties

        /// <summary>
        /// Determines overall status of Operation.
        /// </summary>
        OperationResultStatusEnum OperationResultStatus { get; }

        #endregion

        #region Methods

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

        /// <summary>
        /// Get Xml representation of DataModel Operation Result Collection
        /// </summary>
        /// <returns>Xml representation of DataModel Operation Result Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of DataModel Operation Result Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of DataModel Operation Result Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Adds operation result
        /// </summary>
        /// <param name="resultCode">Result Code</param>
        /// <param name="resultMessage">Result Message</param>
        /// <param name="operationResultType">The type of the result which needs to be added</param>
        /// <returns>The boolean result saying whether add is successful or not</returns>
        Boolean AddOperationResult(String resultCode, String resultMessage, OperationResultType operationResultType);

        /// <summary>
        /// Gets OperationResultCollection representation of DataModel Collection
        /// </summary>
        /// <returns></returns>
        IOperationResultCollection GetOperationResultCollection();

        #endregion
    }
}
