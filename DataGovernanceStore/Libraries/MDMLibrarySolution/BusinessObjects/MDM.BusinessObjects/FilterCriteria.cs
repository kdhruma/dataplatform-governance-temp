using System.Globalization;
using System.IO;
using System.Xml;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.BusinessObjects.Interfaces;

    /// <summary>
    /// Class for filter criteria
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class FilterCriteria : MDMObject, IFilterCriteria
    {
        private String _filteredField;
        private String _filteredFieldValue;

        /// <summary>
        /// Name of the filtered field
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public String FilteredField
        {
            get
            {
                return _filteredField;
            }
            set
            {
                _filteredField = value;
            }
        }

        /// <summary>
        /// Value of the filtered field
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String FilteredFieldValue
        {
            get
            {
                return _filteredFieldValue;
            }
            set
            {
                _filteredFieldValue = value;
            }
        }
        
        #region Xml Serialization

        /// <summary>
        /// Gets Xml representation of current object
        /// </summary>
        /// <returns>Returns Xml representation of current object as string</returns>
        public override String ToXml()
        {
            String result = String.Empty;

            using (StringWriter sw = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                xmlWriter.WriteStartElement("FilterCriteria");

                xmlWriter.WriteAttributeString("Id", Id.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Name", Name);
                xmlWriter.WriteAttributeString("LongName", LongName);
                xmlWriter.WriteAttributeString("FilteredField", FilteredField);
                xmlWriter.WriteAttributeString("FilteredFieldValue", FilteredFieldValue);

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();

                // Get the actual XML
                result = sw.ToString();
            }

            return result;
        }

        /// <summary>
        /// Loads current object from provided XmlNode
        /// </summary>
        /// <param name="node">XmlNode for deserialization</param>
        public void LoadFromXml(XmlNode node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Attributes != null)
            {
                if (node.Attributes["Id"] != null)
                {
                    Id = ValueTypeHelper.Int32TryParse(node.Attributes["Id"].Value, Id);
                }
                if (node.Attributes["Name"] != null)
                {
                    Name = node.Attributes["Name"].Value;
                }
                if (node.Attributes["LongName"] != null)
                {
                    LongName = node.Attributes["LongName"].Value;
                }
                if (node.Attributes["FilteredField"] != null)
                {
                    FilteredField = node.Attributes["FilteredField"].Value;
                }
                if (node.Attributes["FilteredFieldValue"] != null)
                {
                    FilteredFieldValue = node.Attributes["FilteredFieldValue"].Value;
                }
            }
        }

        /// <summary>
        /// Loads current object from provided Xml
        /// </summary>
        /// <param name="xml">Xml string for deserialization</param>
        public void LoadFromXml(String xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode("FilterCriteria");
            if (node != null)
            {
                LoadFromXml(node);
            }
        }

        #endregion
        
    }
}
