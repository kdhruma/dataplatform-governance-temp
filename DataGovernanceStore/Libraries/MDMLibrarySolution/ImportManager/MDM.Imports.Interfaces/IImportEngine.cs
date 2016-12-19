using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Imports.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Jobs;
    using MDM.BusinessObjects.Imports;

    /// <summary>
    /// This interface will be used as base point for import engines
    /// </summary>
    public interface IImportEngine
    {
        Boolean Initialize(Job job, ImportProfile importProfile);

        // Run import engine
        Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IImportSourceData importSourceData);

        // Get current running job
        Job GetCurrentJob();
        
        // Get current running job
        ImportProfile GetCurrentImportProfile();

        // Get execution status of an job
        ExecutionStatus GetExecutionStatus();

    }
}
