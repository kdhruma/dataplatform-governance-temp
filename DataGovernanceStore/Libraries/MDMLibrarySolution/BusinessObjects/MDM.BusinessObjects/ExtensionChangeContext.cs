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
    /// Specifies the change context of an extension.
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class ExtensionChangeContext : ObjectBase, IExtensionChangeContext
    {
        #region Fields

        /// <summary>
        /// Indicates the organization id for extension change context.
        /// </summary>
        private Int32 _organizationId = -1;

        /// <summary>
        /// Indicates the organization name for extension change context.
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Indicates the container id for extension change context.
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Indicates the container name for extension change context.
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Indicates the container type for extension change context
        /// </summary>
        private ContainerType _containerType = ContainerType.Unknown;

        /// <summary>
        /// Indicates the container qualifier name for extension change context
        /// </summary>
        private String _containerQualifierName = String.Empty;

        /// <summary>
        /// Indicates variants change context for a extension
        /// </summary>
        private VariantsChangeContext _variantsChangeContext = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameter-less Constructor
        /// </summary>
        public ExtensionChangeContext()
            : base()
        {
        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public ExtensionChangeContext(String valuesAsXml)
        {
            LoadExtensionChangeContext(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies container id for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Int32 OrganizationId
        {
            get
            {
                return this._organizationId;
            }
            set
            {
                this._organizationId = value;
            }
        }

        /// <summary>
        /// Specifies container name for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public String OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
            }
        }

        /// <summary>
        /// Specifies container id for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int32 ContainerId
        {
            get
            {
                return this._containerId;
            }
            set
            {
                this._containerId = value;
            }
        }

        /// <summary>
        /// Specifies container name for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String ContainerName
        {
            get
            {
                return this._containerName;
            }
            set
            {
                this._containerName = value;
            }
        }

        /// <summary>
        /// Specifies container type for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public ContainerType ContainerType
        {
            get
            {
                return this._containerType;
            }
            set
            {
                this._containerType = value;
            }
        }

        /// <summary>
        /// Specifies container qualifier name for extension change context.
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String ContainerQualifierName
        {
            get
            {
                return this._containerQualifierName;
            }
            set
            {
                this._containerQualifierName = value;
            }
        }

        /// <summary>
        /// Specifies variants change context for a variant tree.
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public VariantsChangeContext VariantsChangeContext
        {
            get
            {
                if (this._variantsChangeContext == null)
                {
                    this._variantsChangeContext = new VariantsChangeContext();
                }

                return this._variantsChangeContext;
            }
            set
            {
                this._variantsChangeContext = value;
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
                    //ExtensionChangeContext node start
                    xmlWriter.WriteStartElement("ExtensionChangeContext");

                    #region write ExtensionChangeContext

                    xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                    xmlWriter.WriteAttributeString("ContainerType", this.ContainerType.ToString());
                    xmlWriter.WriteAttributeString("ContainerQualifierName", this.ContainerQualifierName);

                    #endregion write ExtensionChangeContext

                    #region write variants change context for a variant tree

                    if (this._variantsChangeContext != null)
                    {
                        xmlWriter.WriteRaw(this.VariantsChangeContext.ToXml());
                    }

                    #endregion write variants change context for a variant tree

                    //ExtensionChangeContext node end
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
            if (obj is ExtensionChangeContext)
            {
                ExtensionChangeContext objectToBeCompared = obj as ExtensionChangeContext;

                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                {
                    return false;
                }
                if (this.OrganizationName != objectToBeCompared.OrganizationName)
                {
                    return false;
                }
                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }
                if (this.ContainerName != objectToBeCompared.ContainerName)
                {
                    return false;
                }
                if (this.ContainerType != objectToBeCompared.ContainerType)
                {
                    return false;
                }
                if (this.ContainerQualifierName != objectToBeCompared.ContainerQualifierName)
                {
                    return false;
                }
                if (!this.VariantsChangeContext.Equals(objectToBeCompared.VariantsChangeContext))
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

            hashCode = this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^
                       this.ContainerType.GetHashCode() ^ this.ContainerQualifierName.GetHashCode() ^ this.VariantsChangeContext.GetHashCode();

            return hashCode;
        }

        #region Variants Change Context related methods

        /// <summary>
        /// Gets the variants change context of ExtensionChangeContext.
        /// </summary>
        /// <returns>Variants change context of the ExtensionChangeContext.</returns>
        public IVariantsChangeContext GetVariantsChangeContext()
        {
            if (this._variantsChangeContext == null)
            {
                return null;
            }

            return (IVariantsChangeContext)this.VariantsChangeContext;
        }

        /// <summary>
        /// Sets the variants change context of ExtensionChangeContext.
        /// </summary>
        /// <param name="iVariantsChangeContext">Indicates the variants change context to be set</param>
        public void SetVariantsChangeContext(IVariantsChangeContext iVariantsChangeContext)
        {
            this.VariantsChangeContext = (VariantsChangeContext)iVariantsChangeContext;
        }

        #endregion

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadExtensionChangeContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "ExtensionChangeContext")
                    {
                        #region Read ExtensionChangeContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("OrganizationId"))
                            {
                                this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("OrganizationName"))
                            {
                                this.OrganizationName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("ContainerName"))
                            {
                                this.ContainerName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("ContainerType"))
                            {
                                ContainerType containerType = Core.ContainerType.Unknown;
                                ValueTypeHelper.EnumTryParse<ContainerType>(reader.ReadContentAsString(), true, out containerType);

                                this._containerType = containerType;
                            }
                            if (reader.MoveToAttribute("ContainerQualifierName"))
                            {
                                this.ContainerQualifierName = reader.ReadContentAsString();
                            }
                        }

                        #endregion
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name == "VariantsChangeContext")
                    {
                        #region Read VariantsChangeContext

                        String variantsChangeContextXml = reader.ReadOuterXml();

                        if (!String.IsNullOrEmpty(variantsChangeContextXml))
                        {
                            this.VariantsChangeContext = new VariantsChangeContext(variantsChangeContextXml);
                        }

                        #endregion Read VariantsChangeContext
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