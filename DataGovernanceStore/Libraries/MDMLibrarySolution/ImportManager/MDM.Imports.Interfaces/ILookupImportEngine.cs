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
    /// Interface for Lookup Import Engine
    /// </summary>
    public interface ILookupImportEngine
    {
        Boolean Initialize(Job job, LookupImportProfile lookupImportProfile);

        // Run import engine
        Boolean RunStep(String stepName, StepConfiguration stepConfiguration, ILookupImportSourceData importSourceData);

    }
}
