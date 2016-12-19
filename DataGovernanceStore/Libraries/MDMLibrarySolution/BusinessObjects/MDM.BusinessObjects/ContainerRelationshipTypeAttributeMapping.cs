using System;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping object of Container RelationshipType Attribute mapping
    /// </summary>
    [DataContract]
    public class ContainerRelationshipTypeAttributeMapping : RelationshipTypeAttributeMapping, IContainerRelationshipTypeAttributeMapping, IRelationshipTypeAttributeMapping, IDataModelObject
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
        /// Field for the descriptive long name of container
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field to specify if mapping is specialized for an relationshipType or derived for relationshipType based on entitytype - attribute mapping. 
        /// </summary>
        private Boolean _isSpecialized = false;

        /// <summary>
        /// Field specifying that the particular attribute is auto promotable for current mapping.
        /// </summary>
        private Boolean _autoPromotable = false;

        /// <summary>
        /// Field denoting the original ContainerRelationshipTypeAttributeMapping
        /// </summary>
        private ContainerRelationshipTypeAttributeMapping _originalContainerRelationshipTypeAttributeMapping = null;

        /// <summary>
        /// Field denoting the externalId
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerRelationshipTypeAttributeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container - RelationshipType - Attribute Mapping</param>
        public ContainerRelationshipTypeAttributeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with Container Id, RelationshipType Id, and Attribute Id as input parameters
        /// </summary>
        /// <param name="organizationId">Indicates the organization Id</param>
        /// <param name="containerId">Indicates the Container Id</param>
        /// <param name="relationshipTypeId">Indicates the RelationshipType Id</param>
        /// <param name="attributeId">Indicates the Attribute Id</param>
        public ContainerRelationshipTypeAttributeMapping(Int32 organizationId, Int32 containerId, Int32 relationshipTypeId, Int32 attributeId)
            : base(relationshipTypeId, attributeId)
        {
            this._organizationId = organizationId;
            this._containerId = containerId;
        }

        /// <summary>
        /// Initialize new instance of ContainerRelationshipTypeAttributeMapping from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for ContainerRelationshipTypeAttributeMapping</param>
        public ContainerRelationshipTypeAttributeMapping(String valuesAsXml)
        {
            LoadContainerRelationshipTypeAttributeMapping(valuesAsXml);
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
                return "ContainerRelationshipTypeAttributeMapping";
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
        /// Property denoting the mapped Container Short Name
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Container Name
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get { return _containerLongName; }
            set { _containerLongName = value; }
        }

        /// <summary>
        /// Property denoting if mapping is specialized for an relationshipType or derived for relationshipType based on entitytype - attribute mapping. 
        /// </summary>
        [DataMember]
        public Boolean IsSpecialized
        {
            get { return _isSpecialized; }
            set { _isSpecialized = value; }
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
        /// Property denoting the OriginalContainerRelationshipTypeAttributeMapping
        /// </summary>
        public ContainerRelationshipTypeAttributeMapping OriginalContainerRelationshipTypeAttributeMapping
        {
            get { return _originalContainerRelationshipTypeAttributeMapping; }
            set { _originalContainerRelationshipTypeAttributeMapping = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting the ExternalId property for this Mapping
        /// </summary>
        public new string ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public new ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.ContainerRelationshipTypeAttributeMapping;
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
            String returnXml = String.Empty;

             using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //ContainerRelationshipsAttributeMapping node start
                    xmlWriter.WriteStartElement("ContainerRelationshipTypeAttributeMapping");

                    xmlWriter.WriteAttributeString("Id", this.Id.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                    xmlWriter.WriteAttributeString("ShowAtCreation",this.ShowAtCreation.ToString());
                    xmlWriter.WriteAttributeString("Required", ((this.Required) ? 'Y' : 'N').ToString());
                    xmlWriter.WriteAttributeString("ReadOnly", this.ReadOnly.ToString());
                    xmlWriter.WriteAttributeString("SortOrder", this.SortOrder.ToString());
                    xmlWriter.WriteAttributeString("ShowInline",this.ShowInline.ToString());
                    xmlWriter.WriteAttributeString("AutoPromotable", this.AutoPromotable.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

         /// <summary>
        /// Clone ContainerRelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerRelationshipTypeAttributeMapping object.</returns>
        new public IContainerRelationshipTypeAttributeMapping Clone()
        {
            ContainerRelationshipTypeAttributeMapping clonedContainerRelationshipTypeAttributeMapping = new ContainerRelationshipTypeAttributeMapping();

            clonedContainerRelationshipTypeAttributeMapping.Id = this.Id;
            clonedContainerRelationshipTypeAttributeMapping.Name = this.Name;
            clonedContainerRelationshipTypeAttributeMapping.LongName = this.LongName;
            clonedContainerRelationshipTypeAttributeMapping.Locale = this.Locale;
            clonedContainerRelationshipTypeAttributeMapping.Action = this.Action;
            clonedContainerRelationshipTypeAttributeMapping.AuditRefId = this.AuditRefId;
            clonedContainerRelationshipTypeAttributeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedContainerRelationshipTypeAttributeMapping.OrganizationId = this.OrganizationId;
            clonedContainerRelationshipTypeAttributeMapping.OrganizationName = this.OrganizationName;
            clonedContainerRelationshipTypeAttributeMapping.OrganizationLongName = this.OrganizationLongName;
            clonedContainerRelationshipTypeAttributeMapping.ContainerId = this.ContainerId;
            clonedContainerRelationshipTypeAttributeMapping.ContainerName = this.ContainerName;
            clonedContainerRelationshipTypeAttributeMapping.ContainerLongName = this.ContainerLongName;
            clonedContainerRelationshipTypeAttributeMapping.RelationshipTypeId = this.RelationshipTypeId;
            clonedContainerRelationshipTypeAttributeMapping.RelationshipTypeName = this.RelationshipTypeName;
            clonedContainerRelationshipTypeAttributeMapping.RelationshipTypeLongName = this.RelationshipTypeLongName;
            clonedContainerRelationshipTypeAttributeMapping.AttributeId = this.AttributeId;
            clonedContainerRelationshipTypeAttributeMapping.AttributeName = this.AttributeName;
            clonedContainerRelationshipTypeAttributeMapping.AttributeLongName = this.AttributeLongName;
            clonedContainerRelationshipTypeAttributeMapping.AttributeParentId = this.AttributeParentId;
            clonedContainerRelationshipTypeAttributeMapping.AttributeParentName = this.AttributeParentName;
            clonedContainerRelationshipTypeAttributeMapping.AttributeParentLongName = this.AttributeParentLongName;
            clonedContainerRelationshipTypeAttributeMapping.Required = this.Required;
            clonedContainerRelationshipTypeAttributeMapping.ReadOnly = this.ReadOnly;
            clonedContainerRelationshipTypeAttributeMapping.ShowAtCreation = this.ShowAtCreation;
            clonedContainerRelationshipTypeAttributeMapping.SortOrder = this.SortOrder;
            clonedContainerRelationshipTypeAttributeMapping.ShowInline = this.ShowInline;
            clonedContainerRelationshipTypeAttributeMapping.AutoPromotable = this.AutoPromotable;

            return clonedContainerRelationshipTypeAttributeMapping;
        }

        /// <summary>
        /// Delta Merge of ContainerRelationshipTypeAttributeMapping
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeAttributeMapping">ContainerRelationshipTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerRelationshipTypeAttributeMapping instance</returns>
        public IContainerRelationshipTypeAttributeMapping MergeDelta(IContainerRelationshipTypeAttributeMapping deltaContainerRelationshipTypeAttributeMapping, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IContainerRelationshipTypeAttributeMapping mergedContainerRelationshipTypeAttributeMapping = (returnClonedObject == true) ? deltaContainerRelationshipTypeAttributeMapping.Clone() : deltaContainerRelationshipTypeAttributeMapping;
            mergedContainerRelationshipTypeAttributeMapping.Action = (mergedContainerRelationshipTypeAttributeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainerRelationshipTypeAttributeMapping;
        }

        /// <summary>
        /// Compare container relationship type attribute mapping with current container relationship type attribute mapping .
        /// This method will compare container relationship type attribute mapping .
        /// </summary>
        /// <param name="subsetContainerRelationshipTypeAttributeMapping">
        /// Container relationship type attribute mapping to be compared with current container relationship type attribute mapping.
        /// </param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ContainerRelationshipTypeAttributeMapping subsetContainerRelationshipTypeAttributeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetContainerRelationshipTypeAttributeMapping.Id)
                {
                    return false;
                }

                if (this.OrganizationId != subsetContainerRelationshipTypeAttributeMapping.OrganizationId)
                {
                    return false;
                }

                if (this.ContainerId != subsetContainerRelationshipTypeAttributeMapping.ContainerId)
                {
                    return false;
                }

                if (this.RelationshipTypeId != subsetContainerRelationshipTypeAttributeMapping.RelationshipTypeId)
                {
                    return false;
                }

                if (this.AttributeId != subsetContainerRelationshipTypeAttributeMapping.AttributeId)
                {
                    return false;
                }

                if (this.AttributeParentId != subsetContainerRelationshipTypeAttributeMapping.AttributeParentId)
                {
                    return false;
                }
            }

            if (this.OrganizationName != subsetContainerRelationshipTypeAttributeMapping.OrganizationName)
            {
                return false;
            }

            if (this.ContainerName != subsetContainerRelationshipTypeAttributeMapping.ContainerName)
            {
                return false;
            }

            if (this.RelationshipTypeName != subsetContainerRelationshipTypeAttributeMapping.RelationshipTypeName)
            {
                return false;
            }

            if (this.AttributeName != subsetContainerRelationshipTypeAttributeMapping.AttributeName)
            {
                return false;
            }

            if (this.AttributeParentName != subsetContainerRelationshipTypeAttributeMapping.AttributeParentName)
            {
                return false;
            }

            if (this.ReadOnly != subsetContainerRelationshipTypeAttributeMapping.ReadOnly)
            {
                return false;
            }

            if (this.Required != subsetContainerRelationshipTypeAttributeMapping.Required)
            {
                return false;
            }

            if (this.Action != subsetContainerRelationshipTypeAttributeMapping.Action)
            {
                return false;
            }

            if (this.ShowInline != subsetContainerRelationshipTypeAttributeMapping.ShowInline)
            {
                return false;
            }

            if (this.SortOrder != subsetContainerRelationshipTypeAttributeMapping.SortOrder)
            {
                return false;
            }

            if (this.ShowAtCreation != subsetContainerRelationshipTypeAttributeMapping.ShowAtCreation)
            {
                return false;
            }

            if (this.AutoPromotable != subsetContainerRelationshipTypeAttributeMapping.AutoPromotable)
            {
                return false;
            }

            return true;
        }

        #region Overriden Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ContainerRelationshipTypeAttributeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ContainerRelationshipTypeAttributeMapping)
            {
                ContainerRelationshipTypeAttributeMapping objectToBeCompared = obj as ContainerRelationshipTypeAttributeMapping;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.OrganizationId != objectToBeCompared.OrganizationId)
                {
                    return false;
                }

                if (this.OrganizationName != objectToBeCompared.OrganizationName)
                {
                    return false;
                }

                if (this.OrganizationLongName != objectToBeCompared.OrganizationLongName)
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

                if (this.ContainerLongName != objectToBeCompared.ContainerLongName)
                {
                    return false;
                }

                if (this.IsSpecialized != objectToBeCompared.IsSpecialized)
                {
                    return false;
                }

                if (this.AutoPromotable != objectToBeCompared.AutoPromotable)
                {
                    return false;
                }

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
                this.AutoPromotable.GetHashCode();
        }

        #endregion

        #region IDataModelObject

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
        /// <param name="valuesAsXml">Xml having values for current container relationship type attribute mapping
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <ContainerRelationshipTypeAttribute Id="-1" ContainerId = "4" RelationshipTypeId = "6" AttributeId = "-1" ShowAtCreation = "False" Required = "N" ReadOnly = "False" SortOrder = "0" ShowInline = "False" Action="Create" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadContainerRelationshipTypeAttributeMapping(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <ContainerRelationshipTypeAttribute Id="-1" ContainerId = "4" RelationshipTypeId = "6" AttributeId = "-1" ShowAtCreation = "False" Required = "N" ReadOnly = "False" SortOrder = "0" ShowInline = "False" Action="Create" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerRelationshipTypeAttribute")
                        {
                            #region Read container relationship type attributes

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

                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
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

                                if (reader.MoveToAttribute("SortOrder"))
                                {
                                    this.SortOrder = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("ShowInline"))
                                {
                                    this.ShowInline = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
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