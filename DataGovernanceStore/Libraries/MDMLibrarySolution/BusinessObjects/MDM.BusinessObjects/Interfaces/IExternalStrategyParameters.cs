using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get the external merging process related information.
    /// </summary>
    public interface IExternalStrategyParameters
    {
        /// <summary>
        /// Name of external strategy
        /// </summary>
        String ExternalStrategyName { get; set; }

        /// <summary>
        /// Source attribute
        /// </summary>
        Attribute SourceAttribute { get; set; }

        /// <summary>
        /// Source Entity for merging process
        /// </summary>
        Entity SourceEntity { get; set; }

        /// <summary>
        /// Target Entity for merging process
        /// </summary>
        Entity TargetEntity { get; set; }

        /// <summary>
        /// Target attribute
        /// </summary>
        Attribute TargetAttribute { get; set; }
    }
}
