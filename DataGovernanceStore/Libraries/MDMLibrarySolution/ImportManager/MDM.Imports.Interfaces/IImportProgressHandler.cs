using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Core;

namespace MDM.Imports.Interfaces
{
    /// <summary>
    /// Keeps tracks of the progress made by the import engine.
    /// </summary>
    public interface IImportProgressHandler
    {
        /// <summary>
        /// Get the total number of entities processed so far.
        /// </summary>
        /// <returns></returns>
        Int64 GetTotalEntities();

        /// <summary>
        /// Sets the total number of entities to be processed.
        /// </summary>
        /// <param name="totalEntities"></param>
        void SetTotalEntities(Int64 totalEntities);

        /// <summary>
        /// Updates the entity count for success and failure at the end of a given batch.
        /// </summary>
        /// <param name="successEntities"></param>
        /// <param name="failedEntities"></param>
        void UpdateCompletedEntityBatch(Int64 successEntities, Int64 partialSuccessEntities, Int64 failedEntities);

        /// <summary>
        /// Reset all counts to zero
        /// </summary>
        void ResetEntityCounts();

        /// <summary>
        /// Updates only the successful entities count
        /// </summary>
        /// <param name="successEntities">Indicates the successful entities count</param>
        void UpdateSuccessFulEntities(Int64 successEntities);

        /// <summary>
        /// Updates only the partial successful entities count
        /// </summary>
        /// <param name="partialSuccessEntities">Indicates the partial successful entities count</param>
        void UpdatePartialSuccessFulEntities(Int64 partialSucessEntities);

        /// <summary>
        /// Updates only the failed entities count
        /// </summary>
        /// <param name="failedEntities">Indicates the failed entities count</param>
        void UpdateFailedEntities(Int64 failedEntities);

        
        /// <summary>
        /// Updates only the un-change entities count
        /// </summary>
        /// <param name="unChangeEntities">Indicates the not change entities count</param>
        void UpdateUnChangeEntities(Int64 unChangeEntities);

        /// <summary>
        /// Updates the number of entities that got successfully processed for a given attribute type
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        void UpdateSuccessFulAtttributeBatch(AttributeModelType attType, Int64 successEntities);

        /// <summary>
        /// Updates the number of entities that failed processing for a given attribute type
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        void UpdateFailedAtttributeBatch(AttributeModelType attType, Int64 failedEntities);

        /// <summary>
        /// Update the number of successful and failed entities at the end of a batch for a given attribute type.
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        /// <param name="failedEntities"></param>
        void UpdateCompletedAtttributeBatch(AttributeModelType attType, Int64 successEntities, Int64 failedEntities);

        /// <summary>
        /// Gets the completed entities at a given point of time
        /// </summary>
        /// <returns></returns>
        Int64 GetCompletedEntities();

        /// <summary>
        /// Gets the number of successfully completed entities.
        /// </summary>
        /// <returns></returns>
        Int64 GetSuccessFulEntities();

        /// <summary>
        /// Gets the number of partially completed entities.
        /// </summary>
        /// <returns></returns>
        Int64 GetPartialSuccessFulEntities();

        /// <summary>
        /// Gets the number of failed entities
        /// </summary>
        /// <returns></returns>
        Int64 GetFailedEntities();

        /// <summary>
        /// Gets the number of un-change entities
        /// </summary>
        /// <returns>Returns the number of un-change entities</returns>
        Int64 GetUnChangedEntitiesCount();

        /// <summary>
        /// Gets the number of entities that failed for a given attribute type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        Int64 GetFailedEntitiesForAttributes(AttributeModelType attributeType);
    }
}
