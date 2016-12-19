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
    /// Specifies interface for JigsawReportConfigurationInstance
    /// </summary>
    public interface IJigsawReportConfigurationInstance : IXmlSerializable, ICloneable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        String Url { get; set; }

        /// <summary>
        /// Gets or sets the shared secret key.
        /// </summary>
        String SharedSecretKey { get; set; }

        /// <summary>
        /// Gets or sets the salt key.
        /// </summary>
        String SaltKey { get; set; }
    }
}
