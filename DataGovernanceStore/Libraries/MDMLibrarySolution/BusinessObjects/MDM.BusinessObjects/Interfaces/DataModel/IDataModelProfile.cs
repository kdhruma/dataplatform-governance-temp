using System;

namespace MDM.Interfaces.DataModel
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    
    /// <summary>
    /// Exposes methods or properties to set or get the lookup import profile.
    /// </summary>
    public interface IDataModelImportProfile : IMDMObject
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
        /// Property denoting FileWathcerFolderName
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
        DataModelJobProcessingOptions DataModelJobProcessingOptions { get; set; }

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

