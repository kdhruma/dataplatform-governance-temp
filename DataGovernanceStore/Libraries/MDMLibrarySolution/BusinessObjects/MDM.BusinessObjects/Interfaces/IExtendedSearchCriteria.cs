using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Represents the interface for extended search criteria
    /// </summary>
    public interface IExtendedSearchCriteria : ISearchCriteria
    {
        /// <summary>
        /// Gets or sets the additional catalog ids.
        /// </summary>
        /// <value>
        /// The additional catalog ids.
        /// </value>
        Dictionary<Int32, String> AdditionalCatalogIds { get; set; }

        /// <summary>
        /// Gets or sets the additional entity type ids.
        /// </summary>
        /// <value>
        /// The additional entity type ids.
        /// </value>
        Collection<Int32> AdditionalEntityTypeIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [extend search to additional entity types].
        /// </summary>
        /// <value>
        /// <c>true</c> if [extend search to additional entity types]; otherwise, <c>false</c>.
        /// </value>
        Boolean ExtendSearchToAdditionalEntityTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to search in master container.
        /// </summary>
        /// <value>
        /// <c>true</c> if search in master container; otherwise, <c>false</c>.
        /// </value>
        Boolean IsSearchInMasterContainer { get; set; }

        /// <summary>
        /// Get Xml representation of ExtendedSearchCriteria object
        /// </summary>
        /// <returns>
        /// Xml representation of ExtendedSearchCriteria object
        /// </returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of ExtendedSearchCriteria object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>
        /// Xml representation of ExtendedSearchCriteria object
        /// </returns>
        new String ToXml(ObjectSerialization serialization);
    }
}
