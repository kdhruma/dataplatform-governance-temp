using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies JigsawNode
    /// </summary>
    public class JigsawNode : IJigsawNode
    {
        /// <summary>
        /// Specifies HostName
        /// </summary>
        public String HostName { get; set; }

        /// <summary>
        /// Specifies Port
        /// </summary>
        public Int32 Port { get; set; }
        
        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ReadXml(XmlReader reader)
        {
            HostName = reader.GetAttribute("HostName");
            Port = ValueTypeHelper.Int32TryParse(reader.GetAttribute("Port"), 0);
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("HostName", HostName);
            writer.WriteAttributeString("Port", Port.ToString());
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}