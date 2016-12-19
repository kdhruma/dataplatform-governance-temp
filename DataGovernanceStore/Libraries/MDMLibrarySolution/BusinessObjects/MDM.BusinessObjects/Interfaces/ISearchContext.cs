using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the search context.
    /// </summary>
    public interface ISearchContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Specifies the maximum number of records to return
        /// </summary>
        Int32 MaxRecordsToReturn { get; set; }

        /// <summary>
        /// Represents whether to calculate search scores or not
        /// </summary>
        Boolean CalculateScore { get; set; }

        /// <summary>
        /// Indicates if Search in Taxonomy is enabled
        /// </summary>
        Boolean SearchInTaxonomy { get; set; }

        /// <summary>
        /// Indicates SearchDepth
        /// </summary>
        Int32 SearchDepth { get; set; }

        /// <summary>
        /// Indicates whether to include category path in result or not
        /// </summary>
        Boolean IncludeCategoryPathInResult { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Search Specifications
        /// </summary>
        /// <returns>Xml representation of Search Specifications</returns>
        String ToXml();

        /// <summary>
        /// Set Return AttributeList
        /// </summary>
        /// <param name="iReturnAttributeList">ReturnAttributeList</param>
        void SetReturnAttributeList(Collection<IAttribute> iReturnAttributeList);

        /// <summary>
        /// Get ReturnAttributeList
        /// </summary>
        /// <returns>Collection of IAttribute</returns>
        Collection<IAttribute> GetReturnAttributeList();

        #endregion
    }
}
