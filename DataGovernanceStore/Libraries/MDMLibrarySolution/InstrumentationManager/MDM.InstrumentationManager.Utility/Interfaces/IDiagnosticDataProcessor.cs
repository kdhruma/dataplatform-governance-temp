using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDiagnosticDataProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="diagnosticDataElement"></param>
        /// <returns></returns>
        Boolean Post(IDiagnosticDataElement diagnosticDataElement);
    }
}
