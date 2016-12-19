using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Imports;
    using MDM.Core;

    /// <summary>
    /// Specifies the interface for DDG Import Profile
    /// </summary>
    public interface IDDGImportProfile : IMDMObject
    {
        /// <summary>
        /// Property indicating type of import
        /// </summary>
        String ImportType { get; set; }

        /// <summary>
        /// Property denoting whether input file is enabled or not
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting FileWatcher Folder Name
        /// </summary>
        String FileWatcherFolderName { get; set; }

        /// <summary>
        /// Property denoting Input Specifications about the input file
        /// </summary>
        InputSpecifications InputSpecifications { get; set; }

        /// <summary>
        /// Property denoting ExecutionSteps
        /// </summary>
        ExecutionStepCollection ExecutionSteps { get; set; }

        /// <summary>
        /// Property denoting Job Processing Specifications
        /// </summary>
        DDGJobProcessingOptions DDGJobProcessingOptions { get; set; }
    }
}
