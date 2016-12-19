using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Represents class used to specify information about MDM object
    /// </summary>
    [DataContract]
    public class MDMObjectInfo : MDMObject, IMDMObjectInfo
    {
        /// <summary>
        /// Property defining the type of the MDMObjectInfo
        /// </summary>
        [DataMember]
        public new String ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a clone copy of MDM object info object specifying selected set of properties
        /// </summary>
        /// <returns>Returns a clone copy of MDM object info object specifying selected set of properties</returns>
        public IMDMObjectInfo Clone()
        {
            MDMObjectInfo clonedMdmObjectInfo = new MDMObjectInfo()
            {
                Id = this.Id,
                Name = this.Name,
                ObjectType = this.ObjectType
            };

            return clonedMdmObjectInfo;
        }

        /// <summary>
        /// Get xml representation of MDM object information object
        /// </summary>
        /// <returns>Returns xml representation of MDM object information object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            CultureInfo culture = CultureInfo.InvariantCulture;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //MDMObjectInfo node start
            xmlWriter.WriteStartElement("MDMObjectInfo");

            xmlWriter.WriteAttributeString("Id", this.Id.ToString(culture));
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("ObjectType", this.ObjectType);

            //MDMObjectInfo node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Get xml representation of MDM object information object based on object serialization type
        /// </summary>
        /// <param name="serialization">Indicates the type of object serialization</param>
        /// <returns>Returns xml representation of MDM object information object based on object serialization type</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }
    }
}