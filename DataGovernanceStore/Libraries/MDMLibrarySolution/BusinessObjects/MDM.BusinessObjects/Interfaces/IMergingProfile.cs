using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get merge profile information.
    /// </summary>
    public interface IMergingProfile : ICloneable
    {
        /// <summary>
        /// Indicates the Merge Mode
        /// </summary>
        MergeMode Mode { get; set; }

        /// <summary>
        /// Indicates the Selected Context Flags
        /// </summary>
        HashSet<ExposableMergeContextFlag> SelectedContextFlags { get; set;}
    }
}
