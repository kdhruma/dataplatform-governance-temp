using System;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies an instance of Data model exclusion context
    /// </summary>
    [DataContract]
    public class DataModelExclusionContext : MDMObject, IDataModelExclusionContext
    {
        #region Fields

        /// <summary>
        /// Specifies service type
        /// </summary>
        private MDMServiceType _serviceType = MDMServiceType.UnKnown;

        /// <summary>
        /// Specifies Id of an organization
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Specifies Id of container
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Specifies Id of entity type
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Specifies Id of an attribute
        /// </summary>
        private Int32 _attributeId = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DataModelExclusionContext()
        {

        }

        /// <summary>
        /// Converts XML into current instance
        /// </summary>
        /// <param name="valuesAsXml">Specifies xml representation of instance</param>
        public DataModelExclusionContext(String valuesAsXml)
        {
            LoadDataModelExclusionContext(valuesAsXml);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Specifies service type
        /// </summary>
        [DataMember]
        public MDMServiceType ServiceType
        {
            get
            {
                return this._serviceType;
            }
            set
            {
                this._serviceType = value;
            }
        }

        /// <summary>
        /// Specifies Id of an organization
        /// </summary>
        [DataMember]
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
        /// Specifies Id of container
        /// </summary>
        [DataMember]
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
        /// Specifies Id of entity type
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get
            {
                return this._entityTypeId;
            }
            set
            {
                this._entityTypeId = value;
            }
        }

        /// <summary>
        /// Specifies Id of an attribute
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get
            {
                return this._attributeId;
            }
            set
            {
                this._attributeId = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Gives XMl representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //DataModelExclusionContext node start
                    xmlWriter.WriteStartElement("DataModelExclusionContext");

                    #region write DataModelExclusionContext

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ServiceType", this.ServiceType.ToString());
                    xmlWriter.WriteAttributeString("OrganizationId", this.OrganizationId.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                    xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());

                    #endregion

                    //DataModelExclusionContext node end
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
        public override bool Equals(object obj)
        {
            if (obj is DataModelExclusionContext)
            {
                DataModelExclusionContext objectToBeCompared = obj as DataModelExclusionContext;

                if (this.Id != objectToBeCompared.Id)
                {
                    return false;
                }

                if (this.ServiceType != objectToBeCompared.ServiceType)
                {
                    return false;
                }

                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                {
                    return false;
                }

                if (this.ContainerId != objectToBeCompared.ContainerId)
                {
                    return false;
                }

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                {
                    return false;
                }

                if (this.AttributeId != objectToBeCompared.AttributeId)
                {
                    return false;
                }

                if (this.Locale != objectToBeCompared.Locale)
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
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = this.Id.GetHashCode() ^ this.ServiceType.GetHashCode() ^ this.AuditRefId.GetHashCode() ^ this.OrganizationId.GetHashCode() ^
                       this.ContainerId.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.AttributeId.GetHashCode() ^ this.Locale.GetHashCode();

            return hashCode;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadDataModelExclusionContext(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "DataModelExclusionContext")
                    {
                        #region Read DataModelExclusionContext

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("Id"))
                            {
                                this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                            }

                            if (reader.MoveToAttribute("ServiceType"))
                            {
                                MDMServiceType exclusionType = MDMServiceType.UnKnown;
                                Enum.TryParse(reader.ReadContentAsString(), out exclusionType);
                                this.ServiceType = exclusionType;
                            }

                            if (reader.MoveToAttribute("OrganizationId"))
                            {
                                this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.OrganizationId);
                            }

                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.ContainerId);
                            }

                            if (reader.MoveToAttribute("EntityTypeId"))
                            {
                                this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.EntityTypeId);
                            }

                            if (reader.MoveToAttribute("AttributeId"))
                            {
                                this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.AttributeId);
                            }

                            if (reader.MoveToAttribute("Locale"))
                            {
                                LocaleEnum locale = LocaleEnum.UnKnown;
                                Enum.TryParse(reader.ReadContentAsString(), out locale);
                                this.Locale = locale;
                            }
                        }

                        #endregion
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
    }
}
