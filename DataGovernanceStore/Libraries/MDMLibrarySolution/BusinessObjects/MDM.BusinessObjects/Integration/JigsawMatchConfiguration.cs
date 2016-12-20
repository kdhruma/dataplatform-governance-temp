using System;
using System.Xml;
using System.Xml.Schema;

namespace MDM.BusinessObjects.Integration
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents Jigsaw Match Configuration
    /// </summary>
    public class JigsawMatchConfiguration : IJigsawMatchConfiguration
    {
        /// <summary>
        /// Property denoting the base URL.
        /// </summary>
        public String BaseUrl
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the shared secret key.
        /// </summary>
        public String SharedSecretKey
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the salt key.
        /// </summary>
        public String SaltKey
        {
            get; set;
        }

        /// <summary>
        /// Property denoting the API method.
        /// </summary>
        public String ApiMethod
        {
            get; set;
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ReadXml(XmlReader reader)
        {
            BaseUrl = reader.GetAttribute("BaseUrl");
            ApiMethod = reader.GetAttribute("ApiMethod");
            SharedSecretKey = reader.GetAttribute("SharedSecretKey");
            SaltKey = reader.GetAttribute("SaltKey");
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("BaseUrl", BaseUrl);
            writer.WriteAttributeString("ApiMethod", ApiMethod);
            writer.WriteAttributeString("SharedSecretKey", SharedSecretKey);
            writer.WriteAttributeString("SaltKey", SaltKey);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
