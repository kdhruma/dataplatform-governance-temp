using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MDM.Imports.Interfaces;
using MDM.Core;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// Class to handle the progress in the import engine.
    /// </summary>
    public class ImportProgressHandler : IImportProgressHandler
    {
        #region Private Members
        // multiple threads can update this object.
        internal static Object lockObject = new Object();
        private Int64 _totalEntities = 0;
        private Int64 _completedEntities = 0;
        private Int64 _successEntities = 0;
        private Int64 _failedEntities = 0;
        private Int64 _partialSuccessEntities = 0;
        private Int64 _unChangeEntities = 0;

        // Concurrent dictionary
        Dictionary<AttributeModelType, Int64> failedEntitiesForAttributes = new Dictionary<AttributeModelType, Int64>();
        Dictionary<AttributeModelType, Int64> successEntitiesForAttributes = new Dictionary<AttributeModelType, Int64>();
        #endregion

        // default constructor..
        public ImportProgressHandler()
        {

        }

        #region IImportProgressHandler Members

        /// <summary>
        /// Get the total number of entities processed so far.
        /// </summary>
        /// <returns></returns>
        public Int64 GetTotalEntities()
        {
            return _totalEntities;
        }

        /// <summary>
        /// Sets the total number of entities to be processed.
        /// </summary>
        /// <param name="totalEntities">Indicates the total no. of entities</param>
        public void SetTotalEntities(Int64 totalEntities)
        {
            _totalEntities = totalEntities;
        }

        /// <summary>
        /// Updates only the successful entities count
        /// </summary>
        /// <param name="successEntities">Indicates the successful entities count</param>
        public void UpdateSuccessFulEntities(Int64 successEntities)
        {
            UpdateCompletedEntityBatch(successEntities, 0, 0);
        }

        /// <summary>
        /// Updates only the partial successful entities count
        /// </summary>
        /// <param name="partialSuccessEntities">Indicates the partial successful entities count</param>
        public void UpdatePartialSuccessFulEntities(Int64 partialSuccessEntities)
        {
            UpdateCompletedEntityBatch(0, partialSuccessEntities, 0);
        }

        /// <summary>
        /// Updates only the failed entities count
        /// </summary>
        /// <param name="failedEntities">Indicates the failed entities count</param>
        public void UpdateFailedEntities(Int64 failedEntities)
        {
            UpdateCompletedEntityBatch(0, 0, failedEntities);
        }

        /// <summary>
        /// Updates only the no change entities count
        /// </summary>
        /// <param name="unChangeEntities">Indicates the no change entities count</param>
        public void UpdateUnChangeEntities(Int64 unChangeEntities)
        {
            UpdateCompletedEntityBatch(0, 0, 0, unChangeEntities);
        }

        /// <summary>
        /// Updates the entity count for success and failure at the end of a given batch.
        /// </summary>
        /// <param name="successEntities"></param>
        /// <param name="failedEntities"></param>
        public void UpdateCompletedEntityBatch(Int64 successEntities, Int64 partialSuccessEntities, Int64 failedEntities)
        {
            UpdateCompletedEntityBatch(successEntities, partialSuccessEntities, failedEntities, 0);
        }

        private void UpdateCompletedEntityBatch(Int64 successEntities, Int64 partialSuccessEntities, Int64 failedEntities, Int64 unChangeEntities)
        {
            lock (lockObject)
            {
                _partialSuccessEntities += partialSuccessEntities;
                _successEntities += successEntities;
                _failedEntities += failedEntities;
                _unChangeEntities += unChangeEntities;
                _completedEntities += (successEntities + failedEntities + partialSuccessEntities + unChangeEntities);
            }
        }

        /// <summary>
        /// Clear the count of all entities
        /// </summary>
        public void ResetEntityCounts()
        {
            lock (lockObject)
            {
                _partialSuccessEntities = 0;
                _successEntities = 0;
                _failedEntities = 0;
                _completedEntities = 0;
            }
        }

        /// <summary>
        /// Updates the number of entities that got successfully processed for a given attribute type
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        public void UpdateSuccessFulAtttributeBatch(AttributeModelType attType, Int64 successEntities)
        {
            UpdateCompletedAtttributeBatch(attType, successEntities, 0);
        }

        /// <summary>
        /// Updates the number of entities that failed processing for a given attribute type
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        public void UpdateFailedAtttributeBatch(AttributeModelType attType, Int64 failedEntities)
        {
            UpdateCompletedAtttributeBatch(attType, 0, failedEntities);
        }

        /// <summary>
        /// Update the number of successful and failed entities at the end of a batch for a given attribute type.
        /// </summary>
        /// <param name="attType"></param>
        /// <param name="successEntities"></param>
        /// <param name="failedEntities"></param>
        public void UpdateCompletedAtttributeBatch(AttributeModelType attType, Int64 successEntities, Int64 failedEntities)
        {
            Int64 attributeCount = 0;
            lock (lockObject)
            {
                if (failedEntities > 0)
                {
                    if (failedEntitiesForAttributes.Keys.Contains(attType))
                    {
                        attributeCount = failedEntitiesForAttributes[attType];
                    }
                    failedEntitiesForAttributes[attType] = attributeCount + failedEntities;
                }

                if (successEntities > 0)
                {
                    attributeCount = 0;
                    if (successEntitiesForAttributes.Keys.Contains(attType))
                    {
                        attributeCount = successEntitiesForAttributes[attType];
                    }
                    successEntitiesForAttributes[attType] = attributeCount + successEntities;
                }
            }
        }

        /// <summary>
        /// Gets the completed entities at a given point of time
        /// </summary>
        /// <returns></returns>
        public Int64 GetCompletedEntities()
        {
            return _completedEntities;
        }

        /// <summary>
        /// Gets the number of successfully completed entities.
        /// </summary>
        /// <returns></returns>
        public Int64 GetSuccessFulEntities()
        {
            return _successEntities;
        }

        /// <summary>
        /// Gets the number of partially completed entities.
        /// </summary>
        /// <returns></returns>
        public Int64 GetPartialSuccessFulEntities()
        {
            return _partialSuccessEntities;
        }

        /// <summary>
        /// Gets the number of failed entities
        /// </summary>
        /// <returns></returns>
        public Int64 GetFailedEntities()
        {
            return _failedEntities;
        }

        /// <summary>
        /// Gets the number of un-change entities
        /// </summary>
        /// <returns>Returns the number of un-change entities</returns>
        public Int64 GetUnChangedEntitiesCount()
        {
            return _unChangeEntities;
        }

        /// <summary>
        /// Gets the number of entities that failed for a given attribute type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public Int64 GetFailedEntitiesForAttributes(Core.AttributeModelType attributeType)
        {
            if (failedEntitiesForAttributes.Keys.Contains(attributeType))
            {
                return failedEntitiesForAttributes[attributeType];
            }
            return 0;
        }

        #endregion
    }
}
