using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;

namespace RS.MDM.ConfigurationObjects
{
    using RS.MDM.Configuration;

    /// <summary>
    /// Represents an KeyValue Pair Item
    /// </summary>
    [XmlRoot("KeyValuePairItem")]
    [Serializable()]
    public sealed class KeyValuePairItem : Object
    {
        #region Fields

        /// <summary>
        /// Represents the Value of the KeyValue Pair
        /// </summary>
        private String _value = String.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the Value of the KeyValue Pair
        /// </summary>
        [XmlAttribute("Value")]
        [Category("Properties")]
        [Description("Represents the Value of the KeyValue Pair")]
        [TrackChanges()]
        public String Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        #endregion

        #region Constructors

        #endregion

        #region Serialization & Deserialization

        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            this.SetParent();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Get XML representation of the object
        /// </summary>
        /// <returns>XML representation of KeyValuePair Item</returns>
        public override String ToXml()
        {
            String menuItemXml = String.Empty;

            using (var sw = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(sw))
            {
                //KeyValuePairItem node start
                xmlWriter.WriteStartElement("KeyValuePairItem");

                xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                xmlWriter.WriteAttributeString("UniqueIdentifier", this.UniqueIdentifier);
                xmlWriter.WriteAttributeString("Name", this.Name);
                xmlWriter.WriteAttributeString("Description", this.Description);
                xmlWriter.WriteAttributeString("Tag", this.Tag);
                xmlWriter.WriteAttributeString("InheritedParentUId", this.InheritedParentUId);
                xmlWriter.WriteAttributeString("Value", this.Value);

                //KeyValuePairItem node end
                xmlWriter.WriteEndElement();

                xmlWriter.Flush();

                //Get the actual XML
                menuItemXml = sw.ToString();
            }
            return menuItemXml;
        }

        #endregion
    }
}
