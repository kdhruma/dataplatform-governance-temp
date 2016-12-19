using System;
using System.Xml.Serialization;

namespace MDM.Interfaces
{
    /// <summary>
    /// Represents interface for JigsawMatchConfiguration
    /// </summary>
    public interface IJigsawMatchConfiguration : IXmlSerializable, ICloneable
    {
        /// <summary>
        /// Property denoting the base URL.
        /// </summary>
        String BaseUrl { get; set; }

        /// <summary>
        /// Property denoting the shared secret key.
        /// </summary>
        String SharedSecretKey { get; set; }

        /// <summary>
        /// Property denoting the salt key.
        /// </summary>
        String SaltKey { get; set; }

        /// <summary>
        /// Property denoting the API method.
        /// </summary>
        String ApiMethod { get; set; }
    }
}
