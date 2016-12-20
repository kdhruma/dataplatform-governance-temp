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
    /// Specifies the change context of a relationship.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class RelationshipChangeContext : ObjectBase, IRelationshipChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates the id of relationship.
        /// </summary>
        private Int64 _relationshipId = -1;

        /// <summary>
        /// Indicates the from entity id of relationship.
        /// </summary>
        private Int64 _fromEntityId = -1;

        /// <summary>
        /// Indicates the related entity id of relationship.
        /// </summary>
        private Int64 _relatedEntityId = -1;

        /// <summary>
        /// Indicates the relationship type id of relationship.
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Indicates the relationship type name of relationship.
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Indicates the action for an attribute.
        /// </summary>
        private ObjectAction _action = ObjectAction.Unknown;

        /// <summary>
        /// Indicates attribute change contexts collection for a relationship.
        /// </summary>
        private AttributeChangeContextCollection _attributeChangeContexts = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public RelationshipChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public RelationshipChangeContext(String valuesAsXml)
        {
            LoadRelationshipChangeContext(valuesAsXml);
        }

        /// <summary>
        /// Constructor with relationship id, from entity id, related entity id, relationship type id, relationship type name and object action of relationship change context as input parameters
        /// </summary>
        /// <param name="relationshipId">Indicates relationship id of relationship change context</param>
        /// <param name="fromEntityId">Indicates from entity id of relationship change context</param>
        /// <param name="relatedEntityId">Indicates related entity id of relationship change context</param>
        /// <param name="relationshipTypeId">Indicates relationship type id of relationship change context</param>
        /// <param name="relationshipTypeName">Indicates relationship type name of relationship change context</param>
        /// <param name="objectAction">Indicates object action of relationship change context</param>
        public RelationshipChangeContext(Int64 relationshipId, Int64 fromEntityId, Int64 relatedEntityId, Int32 relationshipTypeId, String relationshipTypeName,ObjectAction objectAction)
        {
            this._relationshipId = relationshipId;
            this._fromEntityId = fromEntityId;
            this._relatedEntityId = relatedEntityId;
            this._relationshipTypeId = relationshipTypeId;
            this._relationshipTypeName = relationshipTypeName;
            this._action = objectAction;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies id of relationship.
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int64 RelationshipId
        {
            get
            {
                return this._relationshipId;
            }
            set
            {
                this._relationshipId = value;
            }
        }

        /// <summary>
        /// Specifies the from entity id of relationship.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Int64 FromEntityId
        {
            get
            {
                return this._fromEntityId;
            }
            set
            {
                this._fromEntityId = value;
            }
        }

        /// <summary>
        /// Specifies the related entity id of relationship.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int64 RelatedEntityId
        {
            get
            {
                return this._relatedEntityId;
            }
            set
            {
                this._relatedEntityId = value;
            }
        }

        /// <summary>
        /// Specifies the relationship type id of relationship.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Int32 RelationshipTypeId
        {
            get
            {
                return this._relationshipTypeId;
            }
            set
            {
                this._relationshipTypeId = value;
            }
        }

        /// <summary>
        /// Specifies the relationship type name of relationship.
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String RelationshipTypeName
        {
            get
            {
                return this._relationshipTypeName;
            }
            set
            {
                this._relationshipTypeName = value;
            }
        }

        /// <summary>
        /// Specifies the action for based on attribute change context.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public ObjectAction Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value;
            }
        }

        /// <summary>
        /// Specifies collection of locale change context including attribute change contexts.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
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
                    //RelationshipChangeContext node start
                    xmlWriter.WriteStartElement("RelationshipChangeContext");

                    #region write RelationshipChangeContext

                    xmlWriter.WriteAttributeString("RelationshipId", this.RelationshipId.ToString());
                    xmlWriter.WriteAttributeString("FromEntityId", this.FromEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());

                    #endregion

                    #region write attribute change context xml

                    if (this._attributeChangeContexts != null)
                    {
                        xmlWriter.WriteRaw(this.AttributeChangeContexts.ToXml());
                    }

                    #endregion write attribute change context xml

                    //RelationshipChangeContext node end
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
            if (obj is RelationshipChangeContext)
            {
                RelationshipChangeContext objectToBeCompared = obj as RelationshipChangeContext;

                if (this.RelationshipId != objectToBeCompared.RelationshipId)
                {
                    return false;
                }
                if (this.FromEntityId != objectToBeCompared.FromEntityId)
                {
                    return false;
                }
                if (this.RelatedEntityId != objectToBeCompared.RelatedEntityId)
                {
                    return false;
                }
                if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                {
                    return false;
                }
                if (this.RelationshipTypeName != objectToBeCompared.RelationshipTypeName)
                {
                    return false;
                }
                if (this.Action != objectToBeCompared.Action)
                {
                    return false;
                }
                if (!this.AttributeChangeContexts.Equals(objectToBeCompared.AttributeChangeContexts))
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

            hashCode = this.RelationshipId.GetHashCode() ^ this.FromEntityId.GetHashCode() ^ this.RelatedEntityId.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^
                       this.RelationshipTypeName.GetHashCode() ^ this.Action.GetHashCode() ^ this.AttributeChangeContexts.GetHashCode();

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
        public void SetVariantsChangeContexts(IAttributeChangeContextCollection iAttributeChangeContexts)
        {
            this.AttributeChangeContexts = (AttributeChangeContextCollection)iAttributeChangeContexts;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadRelationshipChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipChangeContext")
                    {
                        #region Read RelationshipChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("RelationshipId"))
                            {
                                this.RelationshipId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relationshipId);
                            }
                            if (reader.MoveToAttribute("FromEntityId"))
                            {
                                this.FromEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._fromEntityId);
                            }
                            if (reader.MoveToAttribute("RelatedEntityId"))
                            {
                                this.RelatedEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relatedEntityId);
                            }
                            if (reader.MoveToAttribute("RelationshipTypeId"))
                            {
                                this.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._relationshipTypeId);
                            }
                            if (reader.MoveToAttribute("RelationshipTypeName"))
                            {
                                this.RelationshipTypeName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction objectAction = ObjectAction.Unknown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out objectAction);
                                this.Action = objectAction;
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