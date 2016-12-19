using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Specifies interface for Source
    /// </summary>
    public interface ISourceInfo : ICloneable
    {
        /// <summary>
        /// Property denoting SourceId
        /// </summary>
        Int32? SourceId { get; set; }

        /// <summary>
        /// Property denoting SourceEntityId
        /// </summary>
        Int64? SourceEntityId { get; set; }
    }
}
