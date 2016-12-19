using System;
using System.Collections.ObjectModel;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// This interface will be used by the import process to get data from a source.
    /// </summary>
    public interface IDDGImportSourceData
    {
        #region Properties

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        ImportProviderBatchingType BatchingType { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job">Indicates the job which needs to be executed</param>
        /// <param name="ddgImportProfile">Indicates the import profile under which the job will be executed</param>
        /// <returns>True - If source data is initialized, False - If source data is failed to initialize</returns>
        Boolean Initialize(Job job, IDDGImportProfile ddgImportProfile);

        /// <summary>
        /// Returns all DDG Business Object Types
        /// </summary>
        /// <param name="callerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Object Types</returns>
        Collection<ObjectType> GetAllDDGObjectTypesForImport(ICallerContext callerContext);

        /// <summary>
        /// Returns All DDG Objects based on the Object Type 
        /// </summary>
        /// <param name="ddgObject">Indicates the Type of DDG Object</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Objects</returns>
        IBusinessRuleObjectCollection GetAllDDGObjects(ObjectType ddgObject, ICallerContext iCallerContext);

        /// <summary>
        /// Returns All DDG Objects based on the Object Type in a Batch
        /// </summary>
        /// <param name="ddgObject">Indicates the Type of DDG Object</param>
        /// <param name="batchSize">Indicate the size of Batch</param>
        /// <param name="iCallerContext">Indicates the name of the Application and Module which invoked the API</param>
        /// <returns>Collection of DDG Object Types</returns>
        IBusinessRuleObjectCollection GetDDGObjectsNextBatch(ObjectType ddgObject, Int32 batchSize, ICallerContext iCallerContext);

        #endregion Methods
    }
}
