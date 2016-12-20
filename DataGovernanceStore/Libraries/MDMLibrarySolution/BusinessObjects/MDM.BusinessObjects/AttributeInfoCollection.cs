using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;

    /// <summary>
    ///  Specifies information about attribute in attribute change context
    /// </summary>
    [DataContract]
    public class AttributeInfoCollection : InterfaceContractCollection<IAttributeInfo, AttributeInfo>
    {
        #region Fields

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AttributeInfoCollection()
        {

        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public AttributeInfoCollection(String valuesAsXml)
        {
            LoadAttributeInfoCollectionFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gets XML representation of attributeinfo collection object
        /// </summary>
        /// <returns>XML representation of attributeinfo collection object</returns>
        public String ToXml()
        {
            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    // AttributeInfoCollection node start
                    xmlWriter.WriteStartElement("AttributeInfoCollection");

                    foreach (AttributeInfo attribtueInfo in this._items)
                    {
                        xmlWriter.WriteRaw(attribtueInfo.ToXml());
                    }

                    // AttributeInfoCollection node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        /// <summary>
        /// Check whether the attributeinfo collection contains attribute or not
        /// </summary>
        /// <param name="attributeId">Indicates the attribute id</param>
        /// <returns>True - If the attribute having is present in collection</returns>
        public Boolean Contains(Int32 attributeId)
        {
            foreach (AttributeInfo attributeInfo in this._items)
            {
                if (attributeInfo.Id == attributeId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns attributeInfo whose attributeid matches with provided attribute id
        /// </summary>
        /// <param name="attributeId">Indicates the attribute id</param>
        /// <returns>AttributeInfo Object</returns>
        public AttributeInfo GetByAttributeId(Int32 attributeId)
        {
            foreach (AttributeInfo attributeInfo in this._items)
            {
                if (attributeInfo.Id == attributeId)
                {
                    return attributeInfo;
                }
            }

            return null;
        }

        #region Override Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleMapRuleCollection object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is AttributeInfoCollection)
            {
                AttributeInfoCollection objectToBeCompared = obj as AttributeInfoCollection;
                Int32 attributeInfoCollectionUnion = this._items.ToList().Union(objectToBeCompared.ToList()).Count();
                Int32 attributeInfoCollectionIntersect = this._items.ToList().Intersect(objectToBeCompared.ToList()).Count();

                if (attributeInfoCollectionUnion != attributeInfoCollectionIntersect)
                    return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override Int32 GetHashCode()
        {
            Int32 hashCode = 0;

            hashCode = base.GetHashCode();

            foreach (AttributeInfo attributeInfo in this._items)
            {
                hashCode += attributeInfo.GetHashCode();
            }

            return hashCode;
        }

        #endregion Override Methods

        #endregion Public Methods

        #region Private Methods

        private void LoadAttributeInfoCollectionFromXml(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeInfo")
                        {
                            String attributeInfoXml = reader.ReadOuterXml();

                            if (!String.IsNullOrWhiteSpace(attributeInfoXml))
                            {
                                AttributeInfo attributeInfo = new AttributeInfo(attributeInfoXml);

                                if (attributeInfo != null)
                                {
                                    this.Add(attributeInfo);
                                }
                            }
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
