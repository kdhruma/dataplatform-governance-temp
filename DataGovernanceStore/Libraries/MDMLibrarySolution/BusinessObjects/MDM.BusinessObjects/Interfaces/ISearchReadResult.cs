using System;
using System.Data;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get search result and get operation result
    /// </summary>
    public interface ISearchReadResult
    {
        #region Properties

        /// <summary>
        /// Property denoting search result in form of data table, which were retrieved
        /// </summary>
        DataTable DataTable { get; set; }

        /// <summary>
        /// Indicates count of results fetched
        /// </summary>
        Int32 TotalCount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets collection of entity
        /// </summary>
        /// <returns>Collection of entities from the search read result</returns>
        IEntityCollection GetEntities();

        /// <summary>
        /// Gets collection of entity operation result
        /// </summary>
        /// <returns>Collection of entity operation result from the search read result</returns>
        IOperationResult GetOperationResult();

        #endregion Methods
    }
}