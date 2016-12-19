using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get lookup export context information.
    /// </summary>
    public interface ILookupExportContext : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Represents the collection of locales.
        /// </summary>
        Collection<LocaleEnum> LocaleList { get; set; }

        /// <summary>
        /// Represents the lookup export group by order.
        /// </summary>
        LookupExportGroupOrder GroupBy { get; set; }

        /// <summary>
        /// Represents the lookup export file format.
        /// </summary>  
        LookupExportFileFormat FileFormat { get; set; }

        /// <summary>
        /// Represents the maximum number of lookups could be exported per file.
        /// By default value will be 50.
        /// Maximum limit is 255 (Excel work sheet limit per file).
        /// Minimum limit could be 1.
        /// </summary>
        Int32 MaxNoOfLookupsPerFile { get; set; }

        /// <summary>
        /// Represents the maximum number of lookup records can be exported per lookup.
        /// Maximum limit could be 1 million record per lookup
        /// </summary>
        Int32 MaxRecordsPerLookup { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents Lookup export context in Xml format
        /// </summary>
        /// <returns>String representation of Lookup export context object</returns>
        String ToXml();

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        bool Equals(Object obj);

        #endregion
    }
}
