using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.DataModel;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies interface for data model manager.
    /// </summary>
    public interface IDataModelManager
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Fills missing information to data model objects.
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        void FillDataModel(IDataModelObjectCollection iDataModelObjects, ICallerContext iCallerContext);

        /// <summary>
        /// Validates DataModelObjects and populate OperationResults
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be filled up.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <returns>DataModel Operation Result Collection</returns>
        void  Validate(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext);

        /// <summary>
        /// Load original data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to load original object.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        void LoadOriginal(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext);
        
        /// <summary>
        /// Compare and merge data model object collection with current collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be compare and merge.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Returns merged data model objects.</returns>
        void CompareAndMerge(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext);

        /// <summary>
        /// Process data model collection
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be processed.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        /// <returns>Result of operation</returns>
        void Process(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext);

        /// /// <summary>
        /// Processes the Entity cache statuses for data model objects
        /// </summary>
        /// <param name="iDataModelObjects">Collection of data model objects to be process entity cache load.</param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        /// <param name="iCallerContext">Context of caller making call to this API.</param>
        void InvalidateCache(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResult, ICallerContext iCallerContext);

        /// <summary>
        /// Generate and OperationResult Schema for given DataModelObjectCollection
        /// </summary>
        /// <param name="iDataModelObjects"></param>
        /// <param name="operationResult">Collection of DataModel OperationResult</param>
        void PrepareOperationResultsSchema(IDataModelObjectCollection iDataModelObjects, DataModelOperationResultCollection operationResultCollection);

        #endregion
    }
}