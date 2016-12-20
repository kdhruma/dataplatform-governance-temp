using System;

namespace MDM.Imports.Interfaces
{
    using MDM.BusinessObjects.Imports;
    using MDM.Interfaces;

    /// <summary>
    /// Interface for DDGImportEngine
    /// </summary>
    public interface IDDGImportEngine
    {
        /// <summary>
        /// Initialize Import Engine
        /// </summary>
        /// <param name="job">Indicates the job which engine has process</param>
        /// <param name="ddgImportProfile">Indicate the profile information for the job</param>
        /// <returns>True - If engine is started successfully, False - if engine is not started sucessfully</returns>
        Boolean Initialize(IJob job, IDDGImportProfile ddgImportProfile);

        /// <summary>
        /// Run import engine 
        /// </summary>
        /// <param name="stepName">Indicates the step to perform</param>
        /// <param name="stepConfiguration">Indicates the configuration for the current step</param>
        /// <param name="importSourceData">Indicates the import source data</param>
        /// <returns>Flag - True if engine has completed job successfully, False if engine is unable to completed the job successfully</returns>
        Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IDDGImportSourceData ddgImportSourceData);
    }
}
