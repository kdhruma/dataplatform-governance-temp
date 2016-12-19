using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;

    /// <summary>
    /// This interface will be used by the import process to get data from a source.
    /// </summary>
    public interface IDataModelImportSourceData
    {
        #region DataModel Data

        /// <summary>
        /// Provide an opportunity for the source data to initializes itself with some configuration from the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="dataModelImportProfile"></param>
        /// <returns></returns>
        Boolean Initialize(Job job, DataModelImportProfile dataModelImportProfile);

        /// <summary>
        /// Total number of entities available for processing.
        /// </summary>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        Collection<ObjectType> GetDataModelObjectTypesForImport(ICallerContext iCallerContext);

        /// <summary>
        /// Indicates the batching mode the provider supports.
        /// </summary>
        /// <returns></returns>
        ImportProviderBatchingType GetBatchingType();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataModelObjectType"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        Int16 GetBatchCount(ObjectType dataModelObjectType, ICallerContext iCallerContext);

        /// <summary>
        /// Get all data model objects that of given object type
        /// </summary>
        /// <param name="dataModelObject"></param>
        /// <param name="batch"></param>
        /// <param name="iCallerContext"></param>
        /// <returns></returns>
        IDataModelObjectCollection GetAllDataModelObjects(ObjectType dataModelObject, Int16 batch, ICallerContext iCallerContext);

        /// <summary>
        /// Get next available set of data model objects of given batch size.
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="dataModelObject"></param>
        /// <param name="iCallerContext"></param>
        /// <returns>IDataModelObjectCollection</returns>
        IDataModelObjectCollection GetDataModelObjectsNextBatch(Int32 batchSize, ObjectType dataModelObject, ICallerContext iCallerContext);

        /// <summary>
        /// Gets a dictionary of template configuration items
        /// </summary>
        /// <returns>Returns a dictionary of template configuration items</returns>
        IDataModelConfigurationItemDictionary GetConfigurationItems();

        #endregion
    }
}