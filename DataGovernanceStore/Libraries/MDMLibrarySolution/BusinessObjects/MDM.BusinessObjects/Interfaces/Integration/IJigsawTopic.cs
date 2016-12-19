using System;
using System.Xml.Serialization;

namespace MDM.Interfaces
{
    /// <summary>
    /// Specifies interface for Jigsaw Topic
    /// </summary>
    public interface IJigsawTopic : IXmlSerializable, ICloneable
    {
        /// <summary>
        /// Specifies key
        /// </summary>
        String Key { get; set; }

        /// <summary>
        /// Specifies topic
        /// </summary>
        String Topic { get; set; } 
    }
}