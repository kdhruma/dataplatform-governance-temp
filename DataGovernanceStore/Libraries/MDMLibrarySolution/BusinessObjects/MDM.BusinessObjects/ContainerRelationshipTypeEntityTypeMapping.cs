using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.Interfaces;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping object of Container RelationshipType EntityType mapping
    /// </summary>
    [DataContract]
    public class ContainerRelationshipTypeEntityTypeMapping : RelationshipTypeEntityTypeMapping, IContainerRelationshipTypeEntityTypeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for the organization id
        /// </summary>
        private Int32 _organizationId = -1;

        /// <summary>
        /// Field for the descriptive name of organization
        /// </summary>
        private String _organizationName = String.Empty;

        /// <summary>
        /// Field for the descriptive long name of organization
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field for the container id
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Field for the descriptive name of container
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field for the descriptive long name of container
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field to specify if mapping is specialized for an container or derived for container based on relationshipType-entityType mapping. 
        /// </summary>
        private Boolean _isSpecialized = false;

        /// <summary>
        /// Field denoting the Original ContainerRelationshipTypeEntityType Mapping
        /// </summary>
        private ContainerRelationshipTypeEntityTypeMapping _originalContainerRelationshipTypeEntityTypeMapping;

        /// <summary>
        /// Field denoting the ExternalId property of mapping
        /// </summary>
        private string _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameter less Constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Container - RelationshipType - Entity Type Mapping</param>
        public ContainerRelationshipTypeEntityTypeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with organization id, Container Id, Entity Type Id and Attribute Id as input parameters
        /// </summary>
        /// <param name="organizationId">Indicates organization id</param>
        /// <param name="containerId">Indicates the Container Id</param>
        /// <param name="relationshipTypeId">Indicates relationship id</param>
        /// <param name="entityTypeId">Indicates the Entity Type Id</param>
        public ContainerRelationshipTypeEntityTypeMapping(Int32 organizationId, Int32 containerId, Int32 relationshipTypeId, Int32 entityTypeId)
            : base(relationshipTypeId, entityTypeId)
        {
            this._organizationId = organizationId;
            this._containerId = containerId;
        }

        /// <summary>
        /// Constructor with RelationshipType Id and Relationship Name as input parameters
        /// </summary>
        /// <param name="id">Indicates the RelationshipType Id</param>
        /// <param name="relationshipName">Indicates the Relationship Name</param>
        public ContainerRelationshipTypeEntityTypeMapping(Int32 id, String relationshipName) :
            base(id)
        {
            base.Name = relationshipName;
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
                return "ContainerRelationshipTypeEntityTypeMapping";
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
        /// Property denoting the mapped Organization Name
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
        /// Property denoting if mapping is specialized for an container or derived for container based on relationshipType-entityType mapping. 
        /// </summary>
        [DataMember]
        public Boolean IsSpecialized
        {
            get { return _isSpecialized; }
            set { _isSpecialized = value; }
        }

        /// <summary>
        /// Property denoting Original ContainerRelationshipTypeEntityType Mapping.
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMapping OriginalContainerRelationshipTypeEntityTypeMapping
        {
            get { return _originalContainerRelationshipTypeEntityTypeMapping; }
            set { _originalContainerRelationshipTypeEntityTypeMapping = value; }
        }

        #region IDataModelObject

        /// <summary>
        /// Property denoting the ExternalId for this Mapping
        /// </summary>
        public new string ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting the DataModelObjectType for this Mapping
        /// </summary>
        public new ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.ContainerRelationshipTypeEntityTypeMapping;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region IContainerRelationshipTypeEntityTypeMapping

        /// <summary>
        /// Object represents itself as XML
        /// </summary>
        /// <returns>string representation of XML of the object</returns>
        public new String ToXML()
        {
            String xml = string.Empty;

            xml = "<ContainerRelationshipTypeEntityType Id=\"{0}\" ContainerId = \"{1}\" RelationshipTypeId=\"{2}\" EntityTypeId = \"{3}\" DrillDown = \"{4}\" IsDefaultRelation=\"{5}\" Excludable=\"{6}\" ReadOnly=\"{7}\" ValidationRequired=\"{8}\" ShowValidFlagInGrid=\"{9}\" Action=\"{10}\" />    ";

            string retXML = string.Format(xml, this.Id, this.ContainerId, this.RelationshipTypeId, this.EntityTypeId, this.DrillDown, this.IsDefaultRelation, this.Excludable, this.ReadOnly, this.ValidationRequired, this.ShowValidFlagInGrid, this.Action);

            return retXML;
        }

        /// <summary>
        /// Clone ContainerRelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of IContainerRelationshipTypeEntityTypeMapping object.</returns>
        public new IContainerRelationshipTypeEntityTypeMapping Clone()
        {
            ContainerRelationshipTypeEntityTypeMapping clonedContainerRelationshipTypeEntityTypeMapping = new ContainerRelationshipTypeEntityTypeMapping();

            clonedContainerRelationshipTypeEntityTypeMapping.Id = this.Id;
            clonedContainerRelationshipTypeEntityTypeMapping.Name = this.Name;
            clonedContainerRelationshipTypeEntityTypeMapping.LongName = this.LongName;
            clonedContainerRelationshipTypeEntityTypeMapping.Locale = this.Locale;
            clonedContainerRelationshipTypeEntityTypeMapping.Action = this.Action;
            clonedContainerRelationshipTypeEntityTypeMapping.AuditRefId = this.AuditRefId;
            clonedContainerRelationshipTypeEntityTypeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedContainerRelationshipTypeEntityTypeMapping.OrganizationId = this.OrganizationId;
            clonedContainerRelationshipTypeEntityTypeMapping.OrganizationName = this.OrganizationName;
            clonedContainerRelationshipTypeEntityTypeMapping.ContainerId = this.ContainerId;
            clonedContainerRelationshipTypeEntityTypeMapping.ContainerName = this.ContainerName;
            clonedContainerRelationshipTypeEntityTypeMapping.EntityTypeId = this.EntityTypeId;
            clonedContainerRelationshipTypeEntityTypeMapping.EntityTypeName = this.EntityTypeName;
            clonedContainerRelationshipTypeEntityTypeMapping.RelationshipTypeId = this.RelationshipTypeId;
            clonedContainerRelationshipTypeEntityTypeMapping.RelationshipTypeName = this.RelationshipTypeName;
            clonedContainerRelationshipTypeEntityTypeMapping.DrillDown = this.DrillDown;
            clonedContainerRelationshipTypeEntityTypeMapping.Excludable = this.Excludable;
            clonedContainerRelationshipTypeEntityTypeMapping.IsDefaultRelation = this.IsDefaultRelation;
            clonedContainerRelationshipTypeEntityTypeMapping.ReadOnly = this.ReadOnly;
            clonedContainerRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = this.ShowValidFlagInGrid;
            clonedContainerRelationshipTypeEntityTypeMapping.ValidationRequired = this.ValidationRequired;

            return clonedContainerRelationshipTypeEntityTypeMapping;
        }

        /// <summary>
        /// Delta Merge of ContainerRelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeEntityTypeMapping">ContainerRelationshipTypeEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerRelationshipTypeEntityTypeMapping instance</returns>
        public IContainerRelationshipTypeEntityTypeMapping MergeDelta(IContainerRelationshipTypeEntityTypeMapping deltaContainerRelationshipTypeEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IContainerRelationshipTypeEntityTypeMapping mergedContainerRelationshipTypeEntityTypeMapping = (returnClonedObject == true) ? deltaContainerRelationshipTypeEntityTypeMapping.Clone() : deltaContainerRelationshipTypeEntityTypeMapping;

            mergedContainerRelationshipTypeEntityTypeMapping.Action = (mergedContainerRelationshipTypeEntityTypeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainerRelationshipTypeEntityTypeMapping;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ContainerRelationshipTypeEntityTypeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerRelationshipTypeEntityTypeMapping)
            {
                ContainerRelationshipTypeEntityTypeMapping objectToBeCompared = obj as ContainerRelationshipTypeEntityTypeMapping;

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

                if (this.IsSpecialized != objectToBeCompared.IsSpecialized)
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
            return base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.IsSpecialized.GetHashCode();
        }

        #endregion

        #region IDataModelObject Methods

        /// <summary>
        /// Get IDataModelObject for current object.
        /// </summary>
        /// <returns>IDataModelObject</returns>
        public new IDataModelObject GetDataModelObject()
        {
            return this;
        }

        #endregion

        #endregion
    }
}