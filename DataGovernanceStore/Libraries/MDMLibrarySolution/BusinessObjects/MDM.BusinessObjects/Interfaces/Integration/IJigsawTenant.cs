using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MDM.Interfaces
{
    /// <summary>
    /// Represents interface for JigsawTenant
    /// </summary>
    public interface IJigsawTenant : IXmlSerializable, ICloneable
    {
        /// <summary>
        /// Gets or sets the tenant.
        /// </summary>
        String Tenant { get; set; }
    }
}
