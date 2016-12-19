using MDM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get DQM rule.
    /// </summary>
    public interface IDQMRule : ICloneable
    {
        /// <summary>
        /// Short Name
        /// </summary>
        String ShortName { get; set; }

        /// <summary>
        /// Long Name
        /// </summary>
        String LongName { get; set; }
            
        /// <summary>
        /// Long Name
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Rule Xml
        /// </summary>
        String RuleData { get; set; }

        /// <summary>
        /// Weightage
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        Boolean DeleteFlag { get; set; }

        /// <summary>
        /// Audit Ref
        /// </summary>
        Int64 AuditRef { get; set; }

        /// <summary>
        /// Action item for Create/Update/Delete
        /// </summary>
        ObjectAction Action { get; set; }
    }    
}
