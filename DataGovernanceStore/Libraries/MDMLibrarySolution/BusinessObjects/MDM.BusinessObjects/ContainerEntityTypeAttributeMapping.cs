using System;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies mapping object of Container EntityType Attribute
    /// </summary>
    [DataContract]
    public class ContainerEntityTypeAttributeMapping : EntityTypeAttributeMapping, IContainerEntityTypeAttributeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for the organization id
        /// </summary>
        private Int32 _organizationId = -1;

        /// <summary>
        /// Field for the descriptive short name of organization
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Field for the descriptive Long name of organization
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field for the container id
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field for the descriptive short name of container
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field for the descriptive Long name of container
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field to specify if mapping is specialized for an container or derived for container based on entitytype - attribute mapping. 
        /// </summary>
        private Boolean _isSpecialized = false;

        /// <summary>
        /// Field for the externalId
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field specifying that the particular attribute is only inheritable and not overrridden for the current mapping.
        /// </summary>
        private Boolean _inheritableOnly = false;

        /// <summary>
        /// Field specifying that the particular attribute is auto promotable for current mapping.
        /// </summary>
        private Boolean _autoPromotable = false;

        /// <summary>
        /// Field to specify if original ContainerEntityTypeAttributeMapping
        /// </summary>
        private ContainerEntityTypeAttributeMapping _originalContainerEntityTypeAttributeMapping = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerEntityTypeAttributeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container - Entity Type - Attribute Mapping</param>
        public ContainerEntityTypeAttributeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with organization id, Container Id, Entity Type Id and Attribute Id as input parameters
        /// </summary>
        /// <param name="organizationId">Indicates organization id</param>
        /// <param name="containerId">Indicates the Container Id</param>
        /// <param name="entityTypeId">Indicates the Entity Type Id</param>
        /// <param name="attributeId">Indicates the Attribute Id</param>
        public ContainerEntityTypeAttributeMapping(Int32 organizationId, Int32 containerId, Int32 entityTypeId, Int32 attributeId)
            : base(entityTypeId, attributeId)
        {
            this._organizationId = organizationId;
            this._containerId = containerId;
        }

        /// <summary>
        /// Initialize new instance of ContainerEntityTypeAttributeMapping from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for ContainerEntityTypeAttributeMapping</param>
        public ContainerEntityTypeAttributeMapping(String valuesAsXml)
        {
            LoadContainerEntityTypeAttributeMapping(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        public new String ObjectType
        {
            get
            {
                return "ContainerEntityTypeAttributeMapping";
            }
        }

        /// <summary>
        /// Property denoting mapped Organization Id
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        /// <summary>
        /// Property denoting the mapped Organization Short Name
        /// </summary>
        [DataMember]
        public String OrganizationName
        {
            get { return _organizationName; }
            set { _organizationName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Organization Long Name
        /// </summary>
        [DataMember]
        public String OrganizationLongName
        {
            get { return _organizationLongName; }
            set { _organizationLongName = value; }
        }

        /// <summary>
        /// Property denoting mapped Container Id
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        /// <summary>
        /// Property denoting mapped Container Short Name
        /// </summary>
        [DataMember]
        public string ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Container Long Name
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get { return _containerLongName; }
            set { _containerLongName = value; }
        }

        /// <summary>
        /// Property denoting if mapping is specialized for an container or derived for container based on entitytype - attribute mapping. 
        /// </summary>
        [DataMember]
        public Boolean IsSpecialized
        {
            get { return _isSpecialized; }
            set { _isSpecialized = value; }
        }

        /// <summary>
        /// Property denoting if the attribute is inheritable only and not overridden for the current mapping
        /// </summary>
        [DataMember]
        public Boolean InheritableOnly
        {
            get { return _inheritableOnly; }
            set { _inheritableOnly = value; }
        }

        /// <summary>
        /// Property denoting if the attribute is auto promotable for the current mapping
        /// </summary>
        [DataMember]
        public Boolean AutoPromotable
        {
            get { return _autoPromotable; }
            set { _autoPromotable = value; }
        }

        /// <summary>
        /// Property denoting if mapping is specialized for an container or derived for container based on entitytype - attribute mapping. 
        /// </summary>
        public ContainerEntityTypeAttributeMapping OriginalContainerEntityTypeAttributeMapping
        {
            get { return _originalContainerEntityTypeAttributeMapping; }
            set { _originalContainerEntityTypeAttributeMapping = value; }
        }

        #region IDataModelObject Properties
        /// <summary>
        /// Property denoting the ExternalId property for this Mapping
        /// </summary>
        new public string ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        new public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.ContainerEntityTypeAttributeMapping;
            }
        }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Object represents itself as XML
        /// </summary>
        /// <returns>string representation of XML of the object</returns>
        public new String ToXML()
        {
            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ContainerEntityTypeAttribute");
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());

            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
            xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());
            xmlWriter.WriteAttributeString("FK_InheritanceMode", "2");
            xmlWriter.WriteAttributeString("Required", this.Required.ToString());
            xmlWriter.WriteAttributeString("ReadOnly", this.ReadOnly.ToString());
            xmlWriter.WriteAttributeString("ShowAtCreation", this.ShowAtCreation.ToString());
            xmlWriter.WriteAttributeString("Extension", this.Extension.ToString());
            xmlWriter.WriteAttributeString("InheritableOnly", this.InheritableOnly.ToString());
            xmlWriter.WriteAttributeString("AutoPromotable", this.AutoPromotable.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());

            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();
       
            return xml;
        }

        /// <summary>
        /// Compare ContainerEntityTypeAttributeMapping with current ContainerEntityTypeAttributeMapping.
        /// This method will compare ContainerEntityTypeAttributeMapping.
        /// </summary>
        /// <param name="subsetContainerEntityTypeAttributeMapping">
        /// Container entity type attribute mapping to be compared with current container entity type attribute mapping.
        /// </param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ContainerEntityTypeAttributeMapping subsetContainerEntityTypeAttributeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetContainerEntityTypeAttributeMapping.Id)
                    return false;

                if (this.ReferenceId != subsetContainerEntityTypeAttributeMapping.ReferenceId)
                    return false;

                if (this.OrganizationId != subsetContainerEntityTypeAttributeMapping.OrganizationId)
                    return false;

                if (this.ContainerId != subsetContainerEntityTypeAttributeMapping.ContainerId)
                    return false;

                if (this.EntityTypeId != subsetContainerEntityTypeAttributeMapping.EntityTypeId)
                    return false;

                if (this.AttributeId != subsetContainerEntityTypeAttributeMapping.AttributeId)
                    return false;
            }

            if (this.ContainerName != subsetContainerEntityTypeAttributeMapping.ContainerName)
                return false;

            if (this.OrganizationName != subsetContainerEntityTypeAttributeMapping.OrganizationName)
                return false;

            if (this.EntityTypeName != subsetContainerEntityTypeAttributeMapping.EntityTypeName)
                return false;

            if (this.AttributeName != subsetContainerEntityTypeAttributeMapping.AttributeName)
                return false;

            if (this.AttributeParentName != subsetContainerEntityTypeAttributeMapping.AttributeParentName)
                return false;

            if (this.Extension != subsetContainerEntityTypeAttributeMapping.Extension)
                return false;

            if (this.ReadOnly != subsetContainerEntityTypeAttributeMapping.ReadOnly)
                return false;

            if (this.Required != subsetContainerEntityTypeAttributeMapping.Required)
                return false;

            if (this.ShowAtCreation != subsetContainerEntityTypeAttributeMapping.ShowAtCreation)
                return false;

            if (this.SortOrder != subsetContainerEntityTypeAttributeMapping.SortOrder)
                return false;

            if (this.InheritableOnly != subsetContainerEntityTypeAttributeMapping.InheritableOnly)
                return false;

            if (this.AutoPromotable != subsetContainerEntityTypeAttributeMapping.AutoPromotable)
                return false;

            return true;
        }

        /// <summary>
        /// Clone ContainerEntityTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerEntityTypeAttributeMapping object.</returns>
        new public IContainerEntityTypeAttributeMapping Clone()
        {
            ContainerEntityTypeAttributeMapping clonedContainerEntityTypeAttributeMapping = new ContainerEntityTypeAttributeMapping();

            clonedContainerEntityTypeAttributeMapping.Id = this.Id;
            clonedContainerEntityTypeAttributeMapping.Name = this.Name;
            clonedContainerEntityTypeAttributeMapping.LongName = this.LongName;
            clonedContainerEntityTypeAttributeMapping.Locale = this.Locale;
            clonedContainerEntityTypeAttributeMapping.Action = this.Action;
            clonedContainerEntityTypeAttributeMapping.AuditRefId = this.AuditRefId;
            clonedContainerEntityTypeAttributeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedContainerEntityTypeAttributeMapping.OrganizationId = this.OrganizationId;
            clonedContainerEntityTypeAttributeMapping.OrganizationName = this.OrganizationName;
            clonedContainerEntityTypeAttributeMapping.OrganizationLongName = this.OrganizationLongName;
            clonedContainerEntityTypeAttributeMapping.ContainerId = this.ContainerId;
            clonedContainerEntityTypeAttributeMapping.ContainerName = this.ContainerName;
            clonedContainerEntityTypeAttributeMapping.ContainerLongName = this.ContainerLongName;
            clonedContainerEntityTypeAttributeMapping.EntityTypeId = this.EntityTypeId;
            clonedContainerEntityTypeAttributeMapping.EntityTypeName = this.EntityTypeLongName;
            clonedContainerEntityTypeAttributeMapping.EntityTypeLongName = this.EntityTypeLongName;
            clonedContainerEntityTypeAttributeMapping.AttributeId = this.AttributeId;
            clonedContainerEntityTypeAttributeMapping.AttributeName = this.AttributeName;
            clonedContainerEntityTypeAttributeMapping.AttributeLongName = this.AttributeLongName;
            clonedContainerEntityTypeAttributeMapping.AttributeParentId = this.AttributeParentId;
            clonedContainerEntityTypeAttributeMapping.AttributeParentName = this.AttributeParentName;
            clonedContainerEntityTypeAttributeMapping.AttributeParentLongName= this.AttributeParentLongName;
            clonedContainerEntityTypeAttributeMapping.IsSpecialized = this.IsSpecialized;
            clonedContainerEntityTypeAttributeMapping.Required = this.Required;
            clonedContainerEntityTypeAttributeMapping.ReadOnly = this.ReadOnly;
            clonedContainerEntityTypeAttributeMapping.ShowAtCreation = this.ShowAtCreation;
            clonedContainerEntityTypeAttributeMapping.Extension = this.Extension;
            clonedContainerEntityTypeAttributeMapping.InheritableOnly = this.InheritableOnly;
            clonedContainerEntityTypeAttributeMapping.AutoPromotable = this.AutoPromotable;

            return clonedContainerEntityTypeAttributeMapping;
        }

        /// <summary>
        /// Delta Merge of ContainerEntityTypeAttributeMapping
        /// </summary>
        /// <param name="deltaContainerEntityTypeAttributeMapping">ContainerEntityTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged ContainerEntityTypeAttributeMapping instance</returns>
        public IContainerEntityTypeAttributeMapping MergeDelta(IContainerEntityTypeAttributeMapping deltaContainerEntityTypeAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IContainerEntityTypeAttributeMapping mergedContainerEntityTypeAttributeMapping = (returnClonedObject == true) ? deltaContainerEntityTypeAttributeMapping.Clone() : deltaContainerEntityTypeAttributeMapping;

            mergedContainerEntityTypeAttributeMapping.Action = (mergedContainerEntityTypeAttributeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainerEntityTypeAttributeMapping;

        }

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ContainerEntityTypeAttributeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ContainerEntityTypeAttributeMapping)
            {
                ContainerEntityTypeAttributeMapping objectToBeCompared = obj as ContainerEntityTypeAttributeMapping;

                if (!base.Equals(objectToBeCompared))
                    return false;

                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                    return false;

                if (this.OrganizationName != objectToBeCompared.OrganizationName)
                    return false;

                if (this.ContainerId != objectToBeCompared.ContainerId)
                    return false;

                if (this.ContainerName != objectToBeCompared.ContainerName)
                    return false;

                if (this.ContainerLongName != objectToBeCompared.ContainerLongName)
                    return false;

                if (this.IsSpecialized != objectToBeCompared.IsSpecialized)
                    return false;

                if (this.InheritableOnly != objectToBeCompared.InheritableOnly)
                    return false;

                if (this.AutoPromotable != objectToBeCompared.AutoPromotable)
                    return false;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>Hash code of the instance</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.OrganizationLongName.GetHashCode() ^ 
                this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.ContainerLongName.GetHashCode() ^ this.IsSpecialized.GetHashCode() ^
                this.InheritableOnly.GetHashCode() ^ this.AutoPromotable.GetHashCode();
        }

        #endregion

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        new public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current container entity type attribute mapping
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <ContainerEntityTypeAttribute Id="-1" ContainerId="3" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" InheritableOnly = "False" AutoPromotable = "False" Action="Create" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadContainerEntityTypeAttributeMapping(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <ContainerEntityTypeAttribute Id="-1" ContainerId="3" EntityTypeId="29" AttributeId = "4079" SortOrder = "0" FK_InheritanceMode = "2" Required = "N" ReadOnly = "False" ShowAtCreation = "False" Extension = "" Action="Create" />
             */
            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerEntityTypeAttribute")
                        {
                            #region Read ContainerEntityType attributes

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OrganizationId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OrganizationName"))
                                {
                                    this.OrganizationName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeId"))
                                {
                                    this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("AttributeName"))
                                {
                                    this.AttributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("AttributeParentName"))
                                {
                                    this.AttributeParentName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Required"))
                                {
                                    this.Required = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Extension"))
                                {
                                    this.Extension = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("InheritableOnly"))
                                {
                                    this.InheritableOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("AutoPromotable"))
                                {
                                    this.AutoPromotable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                            }

                            #endregion
                        }
                        else
                        {
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
        }

        #endregion

        #endregion
    }
}