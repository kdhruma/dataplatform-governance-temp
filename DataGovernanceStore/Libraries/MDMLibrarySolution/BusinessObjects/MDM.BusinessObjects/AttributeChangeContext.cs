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
    /// Specifies the change context of an attribute.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class AttributeChangeContext : ObjectBase, IAttributeChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates the action for an attribute.
        /// </summary>
        private ObjectAction _action = ObjectAction.Unknown;

        /// <summary>
        /// Indicates the attributeinfo collection
        /// </summary>
        private AttributeInfoCollection _attributeInfoCollection = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public AttributeChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public AttributeChangeContext(String valuesAsXml)
        {
            LoadAttributeChangeContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with attributeInfo and object action of an attribute change context as input parameters
        /// </summary>
        /// <param name="attributeInfo">Indicates the attribute id to be added into list</param>
        /// <param name="objectAction">Indicates object action of attribute change context</param>
        public AttributeChangeContext(AttributeInfo attributeInfo, ObjectAction objectAction)
        {
            if (attributeInfo != null)
            {
                this.AttributeInfoCollection.Add(attributeInfo);
                this.Action = objectAction;
            }
            else
            {
                throw new ArgumentNullException("AttributeInfo");
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public ObjectAction Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        /// <summary>
        /// Specifies changed attribute info for a change context.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public AttributeInfoCollection AttributeInfoCollection
        {
            get
            {
                if (this._attributeInfoCollection == null)
                {
                    this._attributeInfoCollection = new AttributeInfoCollection();
                }

                return this._attributeInfoCollection;
            }
            set
            {
                this._attributeInfoCollection = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    #region Write AttributeChangeContext

                    // AttributeChangeContext node start

                    xmlWriter.WriteStartElement("AttributeChangeContext");

                    #region Write AttributeChangeContext Attributes

                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion Write AttributeChangeContext Attributes


                    #region Write AttributeInfoCollection

                    if (this.AttributeInfoCollection != null)
                    {
                        xmlWriter.WriteRaw(this.AttributeInfoCollection.ToXml());
                    }

                    #endregion Write AttributeInfoCollection

                    // AttributeChangeContext node end


                    xmlWriter.WriteEndElement();

                    #endregion Write AttributeChangeContext

                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Returns the attributeinfo collection available in attribute change context
        /// </summary>
        /// <returns>AttributeInfo collection</returns>
        public IAttributeInfoCollection GetAttributeInfoCollection()
        {
            return (IAttributeInfoCollection)this.AttributeInfoCollection;
        }

        /// <summary>
        /// Sets the attributeinfo collection in attribute change context
        /// </summary>
        /// <returns>Indicates the attributeinfo collection</returns>
        public void SetAttributeInfoCollection(IAttributeInfoCollection iAttributeInfoCollection)
        {
            this.AttributeInfoCollection = iAttributeInfoCollection as AttributeInfoCollection;
        }

        #region Override Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is AttributeChangeContext)
            {
                AttributeChangeContext objectToBeCompared = obj as AttributeChangeContext;

                if (this.Action != objectToBeCompared.Action)
                {
                    return false;
                }

                if (!this.AttributeInfoCollection.Equals(objectToBeCompared.AttributeInfoCollection))
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

            hashCode = this.Action.GetHashCode() ^ this.AttributeInfoCollection.GetHashCode();

            return hashCode;
        }

        #endregion Override Methods

        #endregion Public Methods

        #region Private Methods

        private void LoadAttributeChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeChangeContext")
                    {
                        #region Read AttributeChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction objectAction = ObjectAction.Unknown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out objectAction);
                                this.Action = objectAction;
                            }
                        }

                        #endregion Read AttributeChangeContext
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeInfoCollection")
                    {
                        #region Read AttribtueInfoCollection

                        String attributeInfoCollectionXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(attributeInfoCollectionXml))
                        {
                            AttributeInfoCollection attributeInfoCollection = new AttributeInfoCollection(attributeInfoCollectionXml);

                            if (attributeInfoCollection != null && attributeInfoCollection.Count > 0)
                            {
                                this._attributeInfoCollection = attributeInfoCollection;
                            }
                        }

                        #endregion Read AttribtueInfoCollection
                    }
                    else
                    {
                        //Keep on reading the xml until we reach expected node.
                        reader.Read();
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        #endregion Private Methods

        #endregion Methods
    }
}