using System;

namespace MDM.Interfaces.Exports
{

    /// <summary>
    /// Exposes methods or properties to set or get the context for export profile. 
    /// </summary>
    public interface IExportProfileContext
    {
        
        /// <summary>
        /// Field denoting id of the EntityExportProfile.
        /// </summary>
        Int32 ProfileId { get; set; }

        /// <summary>
        /// Property denoting short name of the EntityExportProfile.
        /// </summary>
        String ShortName { get; set; }
    }
}
