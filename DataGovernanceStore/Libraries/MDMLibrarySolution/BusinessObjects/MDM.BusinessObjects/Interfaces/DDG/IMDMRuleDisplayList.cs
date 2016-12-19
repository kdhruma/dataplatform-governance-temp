using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains IMDMRuleDisplayList information
    /// </summary>
    public interface IMDMRuleDisplayList
    {
        #region Properties

        /// <summary>
        /// Indicates the display type
        /// </summary>
        DisplayType DisplayType { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the Display List Based on Ids
        /// </summary>
        /// <param name="objectIds">Indicates the Ids of Objects</param>
        void SetDisplayListByIds(ICollection<Int32> objectIds);

        /// <summary>
        /// Gets the Ids of Objects which are stored in Display List
        /// </summary>
        /// <returns>Ids of Objects</returns>
        ICollection<Int32> GetDisplayListIds();

        /// <summary>
        /// Sets the Display List Based on Names
        /// </summary>
        /// <param name="objectNames">Indicates the names of Objects</param>
        void SetDisplayListByNames(ICollection<String> objectNames);
      
        /// <summary>
        /// Gets the Names of Objects which are stored in Display List
        /// </summary>
        /// <returns>Names of Objects</returns>
        ICollection<String> GetDisplayListNames();

        #endregion Methods
    }
}
