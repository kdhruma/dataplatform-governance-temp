using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents JigsawTopic
    /// </summary>
    public class JigsawTopic : IJigsawTopic
    {
        /// <summary>
        /// Specifies Key
        /// </summary>
        public String Key { get; set; }

        /// <summary>
        /// Specifies Topic
        /// </summary>
        public String Topic { get; set; }
        
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
            reader.MoveToContent();
            Key = reader.GetAttribute("Key");
            Topic = reader.GetAttribute("Topic");
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Key", Key);
            writer.WriteAttributeString("Topic", Topic);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}