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
    /// Interface for DataModel Import Engine
    /// </summary>
    public interface IDataModelImportEngine
    {
        Boolean Initialize(Job job, DataModelImportProfile DataModelImportProfile);

        // Run import engine
        Boolean RunStep(String stepName, StepConfiguration stepConfiguration, IDataModelImportSourceData importSourceData);

    }
}
