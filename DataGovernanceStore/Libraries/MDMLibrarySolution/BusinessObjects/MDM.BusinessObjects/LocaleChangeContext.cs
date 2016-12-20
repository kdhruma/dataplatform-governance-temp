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
    public class LocaleChangeContext : ObjectBase, ILocaleChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates the locale for change context.
        /// </summary>
        private LocaleEnum _dataLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Indicates collection of attribute change context per data locale.
        /// </summary>
        private AttributeChangeContextCollection _attributeChangeContexts = null;

        /// <summary>
        /// Indicates collection of relationship change context per data locale.
        /// </summary>
        private RelationshipChangeContextCollection _relationshipChangeContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public LocaleChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public LocaleChangeContext(String valuesAsXml)
        {
            LoadLocaleChangeContext(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public LocaleEnum DataLocale
        {
            get
            {
                return this._dataLocale;
            }
            set
            {
                this._dataLocale = value;
            }
        }

        /// <summary>
        /// Specifies collection of attribute change context per data locale.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public AttributeChangeContextCollection AttributeChangeContexts
        {
            get
            {
                if (this._attributeChangeContexts == null)
                {
                    this._attributeChangeContexts = new AttributeChangeContextCollection();
                }

                return this._attributeChangeContexts;
            }
            set
            {
                this._attributeChangeContexts = value;
            }
        }

        /// <summary>
        /// Specifies collection of relationship change context per data locale.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public RelationshipChangeContextCollection RelationshipChangeContexts
        {
            get
            {
                if (this._relationshipChangeContexts == null)
                {
                    this._relationshipChangeContexts = new RelationshipChangeContextCollection();
                }

                return this._relationshipChangeContexts;
            }
            set
            {
                this._relationshipChangeContexts = value;
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
                    //LocaleChangeContext node start
                    xmlWriter.WriteStartElement("LocaleChangeContext");

                    #region write LocaleChangeContext

                    xmlWriter.WriteAttributeString("DataLocale", this.DataLocale.ToString());

                    #endregion

                    #region write attribute change context xml

                    if (this._attributeChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.AttributeChangeContexts.ToXml());
                    }

                    #endregion write attribute change context xml

                    #region write relationship change context xml

                    if (this._relationshipChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.RelationshipChangeContexts.ToXml());
                    }

                    #endregion write relationship change context xml

                    //LocaleChangeContext node end
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
            if (obj is LocaleChangeContext)
            {
                LocaleChangeContext objectToBeCompared = obj as LocaleChangeContext;

                if (this.DataLocale != objectToBeCompared.DataLocale)
                {
                    return false;
                }
                if (!this.AttributeChangeContexts.Equals(objectToBeCompared.AttributeChangeContexts))
                {
                    return false;
                }
                if (!this.RelationshipChangeContexts.Equals(objectToBeCompared.RelationshipChangeContexts))
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

            hashCode = this.DataLocale.GetHashCode() ^ this.AttributeChangeContexts.GetHashCode() ^ this.RelationshipChangeContexts.GetHashCode();

            return hashCode;
        }

        #region Attribute Change Contexts related methods

        /// <summary>
        /// Gets the attribute change contexts per locale
        /// </summary>
        /// <returns>Attribute change context for a locale .</returns>
        public IAttributeChangeContextCollection GetAttributeChangeContexts()
        {
            if (this._attributeChangeContexts == null)
            {
                return null;
            }

            return (IAttributeChangeContextCollection)this.AttributeChangeContexts;
        }

        /// <summary>
        /// Sets the attribute change context per locale.
        /// </summary>
        /// <param name="iAttributeChangeContexts">Indicates the attribute change contexts to be set</param>
        public void SetAttributeChangeContexts(IAttributeChangeContextCollection iAttributeChangeContexts)
        {
            this.AttributeChangeContexts = (AttributeChangeContextCollection)iAttributeChangeContexts;
        }

        #endregion

        #region Relationship Change Contexts related methods

        /// <summary>
        /// Gets the relationship change contexts per locale.
        /// </summary>
        /// <returns>Gets the relationship change contexts per locale.</returns>
        public IRelationshipChangeContextCollection GetRelationshipChangeContexts()
        {
            if (this._relationshipChangeContexts == null)
            {
                return null;
            }

            return (IRelationshipChangeContextCollection)this.RelationshipChangeContexts;
        }

        /// <summary>
        /// Sets the relationship change contexts per locale.
        /// </summary>
        /// <param name="iRelationshipChangeContexts">Indicates the relationship change contexts to be set</param>
        public void SetRelationshipChangeContexts(IRelationshipChangeContextCollection iRelationshipChangeContexts)
        {
            this.RelationshipChangeContexts = (RelationshipChangeContextCollection)iRelationshipChangeContexts;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadLocaleChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "LocaleChangeContext")
                    {
                        #region Read LocaleChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("DataLocale"))
                            {
                                LocaleEnum localeEnum = LocaleEnum.UnKnown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out localeEnum);
                                this.DataLocale = localeEnum;
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "AttributeChangeContexts")
                    {
                        #region Read AttributeChangeContexts

                        String attributeChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(attributeChangeContextsXml))
                        {
                            AttributeChangeContextCollection attributeChangeContexts = new AttributeChangeContextCollection(attributeChangeContextsXml);

                            if (attributeChangeContexts != null && attributeChangeContexts.Count > 0)
                            {
                                this.AttributeChangeContexts = attributeChangeContexts;
                            }
                        }

                        #endregion Read AttributeChangeContexts
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipChangeContexts")
                    {
                        #region Read RelationshipChangeContexts

                        String relationshipChangeContextsXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(relationshipChangeContextsXml))
                        {
                            RelationshipChangeContextCollection relationshipChangeContexts = new RelationshipChangeContextCollection(relationshipChangeContextsXml);

                            if (relationshipChangeContexts != null && relationshipChangeContexts.Count > 0)
                            {
                                this.RelationshipChangeContexts = relationshipChangeContexts;
                            }
                        }

                        #endregion Read RelationshipChangeContexts
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