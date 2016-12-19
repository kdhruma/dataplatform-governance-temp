using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.DQM;

    /// <summary>
    /// Exposes methods or properties to set or get DataQualityIndicator failure information.
    /// </summary>
    public interface IDataQualityIndicatorFailureInfo : IEquatable<DataQualityIndicatorFailureInfo>
    {
        /// <summary>
        /// Denotes code of the error
        /// </summary>
        String FailureMessageCode { get; set; }

        /// <summary>
        /// Denotes actual error message
        /// </summary>
        String FailureMessage { get; set; }

        /// <summary>
        /// Denotes additional params for error 
        /// </summary>
        Collection<Object> Params { get; set; }

        /// <summary>
        /// Denotes the failed Attribute Id
        /// </summary>
        Int32? AttributeId { get; set; }

        /// <summary>
        /// Denotes the failed Relationship Id
        /// </summary>
        Int64? RelationshipId { get; set; }

        /// <summary>
        /// Denotes message Locale Id
        /// </summary>
        Int16? LocaleId { get; set; }
    }
}