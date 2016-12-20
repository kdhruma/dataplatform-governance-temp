using System;
using System.ComponentModel;
using MDM.Interfaces;
using MDM.Core;
using Newtonsoft.Json.Linq;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the matching profile class.
    /// </summary>
    public interface IMatchingProfile : IDQMJobProfile, ICloneable
    {
        #region Fields

        /// <summary>
        /// Indicates ShortName of the profile
        /// </summary>
        String ProfileName { get; set; }

        /// <summary>
        /// Indicates maximum number for Suspects to return.
        /// </summary>
        Int32 MaxReturnRecords { get; set; }

        /// <summary>
        /// Indicates the locale.
        /// </summary>
        LocaleEnum Locale { get; set; }

        /// <summary>
        /// Indicates the entity type identifier.
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Indicates the entity type name
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Indicates the source identifier.
        /// </summary>
        Int32 SourceId { get; set; }

        /// <summary>
        /// Indicates the source name
        /// </summary>
        String SourceName { get; set; }

        /// <summary>
        /// Indicates a value whether to apply container filter.
        /// </summary>
        Boolean ApplyContainerFilter { get; set; }

        /// <summary>
        /// Indicates a value whether to apply category filter.
        /// </summary>
        Boolean ApplyCategoryFilter { get; set; }

        /// <summary>
        /// Indicates a value whether to include pending match review items.
        /// </summary>
        Boolean IncludePendingMatchReviewItems { get; set; }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert matching profile into Json format
        /// </summary>
        /// <returns></returns>
        JObject ToJson();

        #endregion Methods

    }
}