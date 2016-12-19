using System;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping object of Container EntityType mapping
    /// </summary>
    [DataContract]
    public class ContainerEntityTypeMapping : MDMObject, IContainerEntityTypeMapping, IDataModelObject
    {
        #region Fields
        /// <summary>
        /// Field for the Organization id
        /// </summary>
        private Int32 _organizationId = -1;

        /// <summary>
        /// Field for the name of Organization
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Field for the long name of Organization
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field for the container id
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field for the name of container
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field for the long name of container
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field for the entity type id
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// Field for the name of entity type 
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field for the long name of entity type 
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting if the selected entity type can be shown at creation time
        /// </summary>
        private Boolean _showAtCreation = false;

        /// <summary>
        /// Field denoting minimum occurance of entity in different categories in a container.
        /// </summary>
        private Int32 _minimumExtensions = 0;

        /// <summary>
        /// Field denoting maximum occurance of entity in different categories in a container.
        /// </summary>
        private Int32 _maximumExtensions = 0;

        /// <summary>
        /// Field Denoting the original ContainerEntityTypeMapping
        /// </summary>
        private ContainerEntityTypeMapping _originalContainerEntityTypeMapping = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting ContainerEntityTypeMapping key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerEntityTypeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of Container - EntityType Mapping</param>
        public ContainerEntityTypeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with Container Id and Entity Type Id as input parameters
        /// </summary>
        /// <param name="containerId">Indicates the Container Id</param>
        /// <param name="entityTypeId">Indicates the Entity Type Id</param>
        public ContainerEntityTypeMapping(Int32 containerId, Int32 entityTypeId)
        {
            this._containerId = containerId;
            this._entityTypeId = entityTypeId;
        }

        /// <summary>
        /// Initialize new instance of ContainerEntityTypeMapping from Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for ContainerEntityTypeMapping</param>
        public ContainerEntityTypeMapping(String valuesAsXml)
        {
            LoadContainerEntityTypeMapping(valuesAsXml);
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
                return "ContainerEntityTypeMapping";
            }
        }

        /// <summary>
        /// Property denoting mapped Container Id
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get { return _organizationId; }
            set { _organizationId = value; }
        }

        /// <summary>
        /// Property denoting the mapped Container Name
        /// </summary>
        [DataMember]
        public String OrganizationName
        {
            get { return _organizationName; }
            set { _organizationName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Organization long name
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
        /// Property denoting the mapped Container Name
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return _containerName; }
            set { _containerName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Container long name
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get { return _containerLongName; }
            set { _containerLongName = value; }
        }

        /// <summary>
        /// Property denoting mapped Entity Type Id
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return _entityTypeId; }
            set { _entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting the mapped Entity Type Name
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return _entityTypeName; }
            set { _entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped Entity Type long name
        /// </summary>
        [DataMember]
        public String EntityTypeLongName
        {
            get { return _entityTypeLongName; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting show at creation 
        /// </summary>
        [DataMember]
        public Boolean ShowAtCreation
        {
            get { return _showAtCreation; }
            set { _showAtCreation = value; }
        }

        /// <summary>
        /// Property denoting minimum occurance of entity in different categories in a container.
        /// </summary>
        [DataMember]
        public Int32 MinimumExtensions
        {
            get { return _minimumExtensions; }
            set { _minimumExtensions = value; }
        }

        /// <summary>
        /// Property denoting maximum occurance of entity in different categories in a container.
        /// </summary>
        [DataMember]
        public Int32 MaximumExtensions
        {
            get { return _maximumExtensions; }
            set { _maximumExtensions = value; }
        }

        /// <summary>
        /// Property denoting the Original ContainerEntityTypeMapping
        /// </summary>
        public ContainerEntityTypeMapping OriginalContainerEntityTypeMapping
        {
            get { return _originalContainerEntityTypeMapping; }
            set { _originalContainerEntityTypeMapping = value; }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.ContainerEntityTypeMapping;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Object represents itself as XML
        /// </summary>
        /// <returns>Xml representing object</returns>
        public override String ToXml()
        {
            String xml = string.Empty;

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("ContainerEntityType");
            xmlWriter.WriteAttributeString("Id", this.Id.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
            xmlWriter.WriteAttributeString("OrgId", this.OrganizationId.ToString());
            xmlWriter.WriteAttributeString("OrganizationName", this.OrganizationName);
            xmlWriter.WriteAttributeString("OrganizationLongName", this.OrganizationLongName);
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
            xmlWriter.WriteAttributeString("ShowAtCreation", this.ShowAtCreation.ToString());
            xmlWriter.WriteAttributeString("MinimumExtensions", this.MinimumExtensions.ToString());
            xmlWriter.WriteAttributeString("MaximumExtensions", this.MaximumExtensions.ToString());
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
        /// Clone ContainerEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of ContainerEntityTypeMapping object.</returns>
        public IContainerEntityTypeMapping Clone()
        {
            ContainerEntityTypeMapping clonedContainerEntityTypeMapping = new ContainerEntityTypeMapping();

            clonedContainerEntityTypeMapping.Id = this.Id;
            clonedContainerEntityTypeMapping.Name = this.Name;
            clonedContainerEntityTypeMapping.LongName = this.LongName;
            clonedContainerEntityTypeMapping.Locale = this.Locale;
            clonedContainerEntityTypeMapping.Action = this.Action;
            clonedContainerEntityTypeMapping.AuditRefId = this.AuditRefId;
            clonedContainerEntityTypeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedContainerEntityTypeMapping.OrganizationId = this.OrganizationId;
            clonedContainerEntityTypeMapping.OrganizationName = this.OrganizationName;
            clonedContainerEntityTypeMapping.OrganizationLongName = this.OrganizationLongName;
            clonedContainerEntityTypeMapping.ContainerId = this.ContainerId;
            clonedContainerEntityTypeMapping.ContainerName = this.ContainerName;
            clonedContainerEntityTypeMapping.ContainerLongName = this.ContainerLongName;
            clonedContainerEntityTypeMapping.EntityTypeId = this.EntityTypeId;
            clonedContainerEntityTypeMapping.EntityTypeName = this.EntityTypeName;
            clonedContainerEntityTypeMapping.EntityTypeLongName = this.EntityTypeLongName;
            clonedContainerEntityTypeMapping.ShowAtCreation = this.ShowAtCreation;
            clonedContainerEntityTypeMapping.MinimumExtensions = this.MinimumExtensions;
            clonedContainerEntityTypeMapping.MaximumExtensions = this.MaximumExtensions;
            return clonedContainerEntityTypeMapping;
        }

        /// <summary>
        /// Delta Merge of ContainerEntityTypeMapping
        /// </summary>
        /// <param name="deltaContainerEntityTypeMapping">ContainerEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged ContainerEntityTypeMapping instance</returns>
        public IContainerEntityTypeMapping MergeDelta(IContainerEntityTypeMapping deltaContainerEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IContainerEntityTypeMapping mergedContainerEntityTypeMapping = (returnClonedObject == true) ? deltaContainerEntityTypeMapping.Clone() : deltaContainerEntityTypeMapping;

            mergedContainerEntityTypeMapping.Action = (mergedContainerEntityTypeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainerEntityTypeMapping;
        }

        /// <summary>
        /// Compare entity type mapping with current entity type mapping .
        /// This method will compare entity type mapping .
        /// </summary>
        /// <param name="subsetContainerEntityTypeMapping">Container entity type mapping to be compared with current container entity type mapping.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>True : Is both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(ContainerEntityTypeMapping subsetContainerEntityTypeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetContainerEntityTypeMapping.Id)
                    return false;

                if (this.ReferenceId != subsetContainerEntityTypeMapping.ReferenceId)
                    return false;

                if (this.OrganizationId != subsetContainerEntityTypeMapping.OrganizationId)
                    return false;

                if (this.ContainerId != subsetContainerEntityTypeMapping.ContainerId)
                    return false;

                if (this.EntityTypeId != subsetContainerEntityTypeMapping.EntityTypeId)
                    return false;
            }

            if (this.ContainerName != subsetContainerEntityTypeMapping.ContainerName)
                return false;

            if (this.ContainerLongName != subsetContainerEntityTypeMapping.ContainerLongName)
                return false;

            if (this.OrganizationName != subsetContainerEntityTypeMapping.OrganizationName)
                return false;

            if (this.OrganizationLongName != subsetContainerEntityTypeMapping.OrganizationLongName)
                return false;

            if (this.EntityTypeName != subsetContainerEntityTypeMapping.EntityTypeName)
                return false;

            if (this.EntityTypeLongName != subsetContainerEntityTypeMapping.EntityTypeLongName)
                return false;

            if (this.ShowAtCreation != subsetContainerEntityTypeMapping.ShowAtCreation)
                return false;

            if (this.MinimumExtensions != subsetContainerEntityTypeMapping.MinimumExtensions)
                return false;

            if (this.MaximumExtensions != subsetContainerEntityTypeMapping.MaximumExtensions)
                return false;

            return true;
        }

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ContainerEntityTypeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerEntityTypeMapping)
            {
                ContainerEntityTypeMapping objectToBeCompared = obj as ContainerEntityTypeMapping;

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

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                    return false;

                if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                    return false;

                if (this.ShowAtCreation != objectToBeCompared.ShowAtCreation)
                    return false;

                if (this.MinimumExtensions != objectToBeCompared.MinimumExtensions)
                    return false;

                if (this.MaximumExtensions != objectToBeCompared.MaximumExtensions)
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
            return base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.OrganizationLongName.GetHashCode()
                   ^ this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.ContainerLongName.GetHashCode()
                   ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.EntityTypeLongName.GetHashCode()
                   ^ this.ShowAtCreation.GetHashCode() ^ this.MinimumExtensions.GetHashCode() ^ this.MaximumExtensions.GetHashCode();
        }

        #endregion

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize current object with Xml
        /// </summary>
        /// <param name="valuesAsXml">Xml having values for current container entity type mapping
        /// <para>
        /// Sample Xml:
        /// <![CDATA[
        ///     <ContainerEntityType Id="2" ContainerId="2" OrgId = "1" EntityTypeId = "16" Action="Create" />
        /// ]]>
        /// </para>
        /// </param>
        private void LoadContainerEntityTypeMapping(String valuesAsXml)
        {
            #region Sample Xml
            /*
             <ContainerEntityType Id="2" ContainerId="2" OrgId = "1" EntityTypeId = "16" Action="Create" />
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
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ContainerEntityType")
                        {
                            #region Read ContainerEntityType attributes

                            if (reader.HasAttributes)
                            {

                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerId"))
                                {
                                    this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("ContainerName"))
                                {
                                    this.ContainerName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ContainerLongName"))
                                {
                                    this.ContainerLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrgId"))
                                {
                                    this.OrganizationId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("OrganizationName"))
                                {
                                    this.OrganizationName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("OrganizationLongName"))
                                {
                                    this.OrganizationLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("EntityTypeLongName"))
                                {
                                    this.EntityTypeLongName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("ShowAtCreation"))
                                {
                                    this.ShowAtCreation = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                                }

                                if (reader.MoveToAttribute("MinimumExtensions"))
                                {
                                    this.MinimumExtensions= ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }

                                if (reader.MoveToAttribute("MaximumExtensions"))
                                {
                                    this.MaximumExtensions = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
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