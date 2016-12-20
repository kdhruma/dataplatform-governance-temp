using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    
    /// <summary>
    /// Exposes methods or properties to set or get lookup import profile related information.
    /// </summary>
    public interface ILookupImportProfile : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property indicating type of import
        /// </summary>
        String ImportType { get; set; }

        /// <summary>
        /// Property denoting whether input file is enabled or not.
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting folder name for file watcher import
        /// </summary>
        String FileWatcherFolderName { get; set; }

        /// <summary>
        /// Property denoting ExecutionSteps
        /// </summary>
        ExecutionStepCollection ExecutionSteps { get; set; }

        /// <summary>
        /// Property denoting specifications about the input file
        /// </summary>
        InputSpecifications InputSpecifications { get; set; }

        /// <summary>
        /// Property denoting lookup job processing options 
        /// </summary>
        LookupJobProcessingOptions LookupJobProcessingOptions { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents ILookupImportProfile  in Xml format
        /// </summary>
        /// <returns>
        /// ILookupImportProfile  in Xml format
        /// </returns>
        String ToXml();

        #endregion

    }
}

