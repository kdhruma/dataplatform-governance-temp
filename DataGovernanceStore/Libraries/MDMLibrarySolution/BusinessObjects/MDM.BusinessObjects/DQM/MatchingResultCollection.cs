using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MatchingResultCollection : InterfaceContractCollection<IMatchingResult, MatchingResult>, IMatchingResultCollection
    {
        #region Constructors

		/// <summary>
        /// Initializes a new instance of the MergeResult Collection
        /// </summary>
        public MatchingResultCollection()
        { }

        /// <summary>
        /// Initialize MergeResult collection from IList
        /// </summary>
        /// <param name="mergeResultList">Source items</param>
        public MatchingResultCollection(IList<MatchingResult> mergeResultList)
        {
            this._items = new Collection<MatchingResult>(mergeResultList);
        } 

	    #endregion

        #region ICloneable Members

        /// <summary>
        /// Clone MergeResult collection
        /// </summary>
        /// <returns>Cloned MergeResult collection object</returns>
        public object Clone()
        {
            MatchingResultCollection clonedResults = new MatchingResultCollection();

            if (this._items != null && this._items.Count > 0)
            {
                foreach (MatchingResult item in this._items)
                {
                    MatchingResult clonedItem = item.Clone() as MatchingResult;
                    clonedResults.Add(clonedItem);
                }
            }

            return clonedResults;
        }

        #endregion
        
        /// <summary>
        /// Gets the first matching result which satisfies the given condition.
        /// </summary>
        /// <param name="booleanCondition">The boolean condition.</param>
        /// <returns>Matching Result</returns>
        public MatchingResult GetOne(Func<MatchingResult, Boolean> booleanCondition)
        {
            foreach (MatchingResult matchingResult in _items)
            {
                if (booleanCondition(matchingResult))
                {
                    return matchingResult;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all matching results which satisfy the passed condition.
        /// </summary>
        /// <param name="booleanCondition">The boolean condition.</param>
        /// <returns>Matching Result Collection</returns>
        public MatchingResultCollection GetAll(Func<MatchingResult, Boolean> booleanCondition)
        {
            MatchingResultCollection resultCollection = new MatchingResultCollection();

            foreach (MatchingResult matchingResult in _items)
            {
                if (booleanCondition(matchingResult))
                {
                    resultCollection.Add(matchingResult);
                }
            }
            return resultCollection;
        }

    }
}
