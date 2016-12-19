using ProtoBuf;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies information about attribute in attribute change context
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class AttributeInfo : MDMObject, IAttributeInfo
    {
        #region Fields

        /// <summary>
        /// Field denoting the attribute parent id
        /// </summary>
        private Int32 _attributeParentId = -1;

        /// <summary>
        /// Field denoting the attribute parent name
        /// </summary>
        private String _attributeParentName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the attribute parent id
        /// </summary>
        [DataMember]
        public Int32 AttributeParentId
        {
            get { return _attributeParentId; }
            set { _attributeParentId = value; }
        }

        /// <summary>
        /// Property denoting the attribute parent name
        /// </summary>
        [DataMember]
        public String AttributeParentName
        {
            get { return _attributeParentName; }
            set { _attributeParentName = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AttributeInfo()
            : base()
        {

        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AttributeInfo(String valuesAsXml)
        {
            LoadAttributeInfo(valuesAsXml);
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="attributeParentId">Indicates the attribute parent id</param>
        /// <param name="attributeParentName">Indicates the attribute parent name</param>
        /// <param name="attributeId">Indicates the attribute id</param>
        /// <param name="attributeName">Indicates the attribute name</param>
        public AttributeInfo(Int32 attributeId, String attributeName, Int32 attributeParentId, String attributeParentName)
        {
            this.Id = attributeId;
            this.Name = attributeName;
            this.AttributeParentId = attributeParentId;
            this.AttributeParentName = attributeParentName;
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Gets XML representation of attribute info object
        /// </summary>
        /// <returns>XML representation of attribute info object</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // AttribtueInfo node start
                    xmlWriter.WriteStartElement("AttributeInfo");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("Name", this.Name);
                    xmlWriter.WriteAttributeString("AttributeParentId", this.AttributeParentId.ToString());
                    xmlWriter.WriteAttributeString("AttributeParentName", this.AttributeParentName);

                    // AttribtueInfo node end
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is AttributeInfo)
            {
                AttributeInfo objectToBeCompared = obj as AttributeInfo;

                if (!base.Equals(obj))
                {
                    return false;
                }

                if (this.AttributeParentId != objectToBeCompared.AttributeParentId)
                {
                    return false;
                }

                if (String.Compare(this.AttributeParentName, objectToBeCompared.AttributeParentName) != 0)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode() + this.AttributeParentId.GetHashCode() ^ this.AttributeParentName.GetHashCode();

            return hashCode;
        }

        #endregion Override Methods

        /// <summary>
        /// Gets a cloned instance of the current MDMRule object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRule object</returns>
        public AttributeInfo Clone()
        {
            AttributeInfo clonedAttributeInfo = new AttributeInfo();

            clonedAttributeInfo.Id = this.Id;
            clonedAttributeInfo.Name = this.Name;
            clonedAttributeInfo.AttributeParentId = this.AttributeParentId;
            clonedAttributeInfo.AttributeParentName = this.AttributeParentName;

            return clonedAttributeInfo;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadAttributeInfo(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeInfo")
                        {
                            #region Read AttributeInfo

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeParentId"))
                                {
                                    this.AttributeParentId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.AttributeParentId);
                                }

                                if (reader.MoveToAttribute("AttributeParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                reader.Read();
                            }

                            #endregion Read AttributeInfo
                        }
                        else
                        {
                            //Keep on reading the xml until we reach expected node.
                            reader.Read();
                        }
                    }
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}
