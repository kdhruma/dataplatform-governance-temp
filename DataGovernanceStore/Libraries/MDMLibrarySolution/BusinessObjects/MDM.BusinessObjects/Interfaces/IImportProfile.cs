using System;
using MDM.BusinessObjects.Imports;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the import profile related information.
    /// </summary>
    public interface IImportProfile : IMDMObject
    {
        /// <summary>
        /// Property denoting ImportType
        /// </summary>
        String ImportType { get; set; }

        /// <summary>
        /// Property denoting ImportFile is enabled or not
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting FileWatcerFolderName
        /// </summary>
        String FileWatcherFolderName { get; set; }

        /// <summary>
        /// Proeprty denoting Providers
        /// </summary>
        ProviderCollection Providers { get; set; }

        /// <summary>
        /// Property denoting InputSpecification
        /// </summary>
        InputSpecifications InputSpecifications { get; set; }

        /// <summary>
        /// Property denoting MappingSpecification
        /// </summary>
        MappingSpecifications MappingSpecifications { get; set; }

        /// <summary>
        /// Property denoting ProcessingSpecifications
        /// </summary>
        ProcessingSpecifications ProcessingSpecifications { get; set; }

        /// <summary>
        /// Property denoting ExecutionSteps
        /// </summary>
        ExecutionStepCollection ExecutionSteps { get; set; }

        /// <summary>
        /// Property denoting UI Profile
        /// </summary>
        String UIProfile { get; set; }
    }
}
