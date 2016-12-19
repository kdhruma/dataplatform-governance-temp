using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    public interface IImportComplexAttribute
    {
        /// <summary>
        /// Total number of attribute values available for processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetDataCount();

        /// <summary>
        /// Provides a seed for the caller to start from.
        /// </summary>
        /// <returns></returns>
        Int64 GetDataSeed();

        /// <summary>
        /// If seed is provided, this indicates the endpoint for the caller to stop processing.
        /// </summary>
        /// <returns></returns>
        Int64 GetEndPoint();

        /// <summary>
        /// Indicates the batch size for the worker thread.
        /// </summary>
        /// <returns></returns>
        Int32 GetDataBatchSize();

        /// <summary>
        /// Gets the data from a given complex attribute staging table and returns a RS table back.
        /// </summary>
        /// <param name="startPK"></param>
        /// <param name="endPK"></param>
        /// <param name="application"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        Table GetDataBatch(String staginTableName, Int64 startPK, Int64 endPK, MDMCenterApplication application, MDMCenterModules module);
    }
}
