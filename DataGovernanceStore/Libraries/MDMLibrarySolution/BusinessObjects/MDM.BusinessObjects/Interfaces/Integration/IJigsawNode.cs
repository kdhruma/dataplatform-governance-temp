using System;
using System.Xml.Serialization;

namespace MDM.Interfaces
{
    /// <summary>
    /// Specifies interface for JigsawNode
    /// </summary>
    public interface IJigsawNode : IXmlSerializable, ICloneable
    {
        /// <summary>
        /// Specifies HostName
        /// </summary>
        String HostName { get; set; }

        /// <summary>
        /// Specifies Port
        /// </summary>
        Int32 Port { get; set; } 
    }
}