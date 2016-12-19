using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces.Exports
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the lookup export scope.
    /// </summary>
    public interface ILookupExportScope : IMDMObject
    {
        /// <summary>
        /// Captures export Mask of the lookup object
        /// </summary>
        String ExportMask
        {
            get;
            set;
        }
    
        /// <summary>
        /// Represents LookupExport Scope object in Xml format
        /// </summary>
        /// <returns>String representation of current  LookupExport Scope object</returns>
        String ToXml();
    }
}
