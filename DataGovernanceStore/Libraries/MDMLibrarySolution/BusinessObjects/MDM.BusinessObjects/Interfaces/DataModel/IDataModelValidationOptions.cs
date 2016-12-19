using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Represents interface for storing data model validation options
    /// </summary>
    public interface IDataModelValidationOptions
    {
        /// <summary>
        /// Denotes whether lookup dependencies validation is required
        /// </summary>
        Boolean ValidateLookupDependencies { get; set; }

        /// <summary>
        /// Denotes whether UOM dependencies validation is required
        /// </summary>
        Boolean ValidateUomDependencies { get; set; }
    }
}