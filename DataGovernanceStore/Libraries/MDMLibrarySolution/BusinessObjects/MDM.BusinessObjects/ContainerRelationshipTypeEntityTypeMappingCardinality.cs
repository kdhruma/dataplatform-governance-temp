using MDM.BusinessObjects.Interfaces;
using MDM.Core;
using MDM.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the mapping object of Container RelationshipType EntityType Mapping Cardinality
    /// </summary>
    [DataContract]
    public class ContainerRelationshipTypeEntityTypeMappingCardinality : RelationshipTypeEntityTypeMappingCardinality, IContainerRelationshipTypeEntityTypeMappingCardinality, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for the CatalogRelTypeNodeType Id
        /// </summary>
        private Int32 _containerRelationshipTypeEntityTypeId = -1;

        /// <summary>
        /// Field for the Orgnization Id
        /// </summary>
        private Int32 _organizationId = 0;

        /// <summary>
        /// Field for the Organization name
        /// </summary>
        private String _organizationName= String.Empty;

        /// <summary>
        /// Field for the Organization long name
        /// </summary>
        private String _organizationLongName = String.Empty;

        /// <summary>
        /// Field for the Container Id
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field for the Container name
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field for the Container long name
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Field to specify if mapping is specialized for container or derived from entity type and relationship type mapping. 
        /// </summary>
        private Boolean _isSpecialized = false;

        /// <summary>
        /// Field for the ExternalId
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field for the Original Container RelationshipType EntityType Mapping Cardinality
        /// </summary>
        private ContainerRelationshipTypeEntityTypeMappingCardinality _originalContainerRelationshipTypeEntityTypeMappingCardinality = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingCardinality()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of RelationshipType EntityType Mapping Cardinality</param>
        public ContainerRelationshipTypeEntityTypeMappingCardinality(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with OrgnizationId, ContainerId, RelationshipType Id, Entity Type Id and To Entity Type Id as input parameters
        /// </summary>
        /// <param name="OrgnizationId">Indicates Orgnization id</param>
        /// <param name="ContainerId">Indicates Container id</param>
        /// <param name="relationshipTypeId">Indicates RelationshipType Id</param>
        /// <param name="entityTypeId">Indicates Entity Type Id</param>
        /// <param name="toEntityTypeId">Indicates Related Entity type id</param>
        public ContainerRelationshipTypeEntityTypeMappingCardinality(Int32 OrgnizationId, Int32 ContainerId, Int32 relationshipTypeId, Int32 entityTypeId, Int32 toEntityTypeId)
        {
            this._organizationId = OrgnizationId;
            this._containerId = ContainerId;
            this.RelationshipTypeId = relationshipTypeId;
            this.EntityTypeId = entityTypeId;
            this.ToEntityTypeId = toEntityTypeId;
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
                return "ContainerRelationshipTypeEntityTypeMappingCardinality";
            }
        }

        /// <summary>
        /// Property defining the CatalogRelTypeNodeType Id
        /// </summary>
        [DataMember]
        public Int32 ContainerRelationshipTypeEntityTypeId
        {
            get
            {
                return _containerRelationshipTypeEntityTypeId;
            }
            set
            {
                _containerRelationshipTypeEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property defining the Organization Id
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        {
            get
            {
                return _organizationId;
            }
            set
            {
                _organizationId = value;
            }
        }

        /// <summary>
        /// Property defining the Organization Name
        /// </summary>
        [DataMember]
        public String OrganizationName
        {
            get
            {
                return _organizationName;
            }
            set
            {
                _organizationName = value;
            }
        }

        /// <summary>
        /// Property defining the Organization long name
        /// </summary>
        [DataMember]
        public String OrganizationLongName
        {
            get
            {
                return _organizationLongName;
            }
            set
            {
                _organizationLongName = value;
            }
        }

        /// <summary>
        /// Property defining the Container Id
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                _containerId = value;
            }
        }

        /// <summary>
        /// Property defining the Container Name
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get
            {
                return _containerName;
            }
            set
            {
                _containerName = value;
            }
        }

        /// <summary>
        /// Property defining the Container long name
        /// </summary>
        [DataMember]
        public String ContainerLongName
        {
            get
            {
                return _containerLongName;
            }
            set
            {
                _containerLongName = value;
            }
        }

        /// <summary>
        /// Specifies if mapping is specialized for container or derived from entity type and relationship type mapping. 
        /// </summary>
        [DataMember]
        public Boolean IsSpecialized
        {
            get { return _isSpecialized; }
            set { _isSpecialized = value; }
        }

        /// <summary>
        /// Property denoting Original Container RelationshipType EntityType Mapping Cardinality.
        /// </summary>
        public ContainerRelationshipTypeEntityTypeMappingCardinality OriginalContainerRelationshipTypeEntityTypeMappingCardinality
        {
            get { return _originalContainerRelationshipTypeEntityTypeMappingCardinality; }
            set { _originalContainerRelationshipTypeEntityTypeMappingCardinality = value; }
        }

        #region IDataModelObject

        /// <summary>
        /// Property denoting the ExternalId for this Mapping
        /// </summary>
        public new String ExternalId
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
                return MDM.Core.ObjectType.ContainerRelationshipTypeEntityTypeMappingCardinality;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region IContainerRelationshipTypeEntityTypeMappingCardinality

        /// <summary>
        /// Get Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </summary>
        /// <returns>
        /// Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </returns>
        public new String ToXML()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //ContainerRelationshipTypeEntityTypeMappingCardinality node start
            xmlWriter.WriteStartElement("ContainerRelationshipTypeEntityTypeMappingCardinality");
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
            xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
            xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntitTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("EntitTypeLongName", this.EntityTypeLongName);
            xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);
            xmlWriter.WriteAttributeString("RelationshipTypeLongName", this.RelationshipTypeLongName);

            xmlWriter.WriteStartElement("Cardinality");
            xmlWriter.WriteAttributeString("RelationshipTypeEntityTypeCardinalityId", this.Id.ToString());
            xmlWriter.WriteAttributeString("ContainerRelationshipTypeEntityTypeId", this.ContainerRelationshipTypeEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeId", this.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeName", this.ToEntityTypeName);
            xmlWriter.WriteAttributeString("MinRelationships", this.MinRelationships.ToString());
            xmlWriter.WriteAttributeString("MaxRelationships", this.MaxRelationships.ToString());

            xmlWriter.WriteEndElement();

            //ExportSubscriber node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// Clone ContainerRelationshipTypeEntityTypeMappingCardinality object
        /// </summary>
        /// <returns>
        /// Cloned copy of IContainerRelationshipTypeEntityTypeMappingCardinality object.
        /// </returns>
         public new IContainerRelationshipTypeEntityTypeMappingCardinality Clone()
        {
            ContainerRelationshipTypeEntityTypeMappingCardinality clonedContainerRelationshipTypeEntityTypeMappingCardinality = new ContainerRelationshipTypeEntityTypeMappingCardinality();

            clonedContainerRelationshipTypeEntityTypeMappingCardinality.Id = this.Id;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.Name = this.Name;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.LongName = this.LongName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.Locale = this.Locale;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.Action = this.Action;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.AuditRefId = this.AuditRefId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.ExtendedProperties = this.ExtendedProperties;

            clonedContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationId = this.OrganizationId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.OrganizationName = this.OrganizationName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.ContainerId = this.ContainerId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.ContainerName = this.ContainerName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = this.RelationshipTypeId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = this.RelationshipTypeName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeId = this.EntityTypeId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.EntityTypeName = this.EntityTypeName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = this.ToEntityTypeId;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = this.ToEntityTypeName;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.MinRelationships = this.MinRelationships;
            clonedContainerRelationshipTypeEntityTypeMappingCardinality.MaxRelationships = this.MaxRelationships;

            return clonedContainerRelationshipTypeEntityTypeMappingCardinality;
        }

        /// <summary>
        /// Delta Merge of ContainerRelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeEntityTypeMappingCardinality">ContainerRelationshipTypeEntityTypeMappingCardinality that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IContainerRelationshipTypeEntityTypeMappingCardinality instance</returns>
        public IContainerRelationshipTypeEntityTypeMappingCardinality MergeDelta(IContainerRelationshipTypeEntityTypeMappingCardinality deltaContainerRelationshipTypeEntityTypeMappingCardinality, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IContainerRelationshipTypeEntityTypeMappingCardinality mergedContainerRelationshipTypeEntityTypeMappingCardinality = (returnClonedObject == true) ? deltaContainerRelationshipTypeEntityTypeMappingCardinality.Clone() : deltaContainerRelationshipTypeEntityTypeMappingCardinality;

            mergedContainerRelationshipTypeEntityTypeMappingCardinality.Action = (mergedContainerRelationshipTypeEntityTypeMappingCardinality.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedContainerRelationshipTypeEntityTypeMappingCardinality;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">ContainerRelationshipTypeEntityTypeMappingCardinality object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is ContainerRelationshipTypeEntityTypeMappingCardinality)
            {
                ContainerRelationshipTypeEntityTypeMappingCardinality objectToBeCompared = obj as ContainerRelationshipTypeEntityTypeMappingCardinality;

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

                if (this.RelationshipTypeId != objectToBeCompared.RelationshipTypeId)
                    return false;

                if (this.RelationshipTypeName != objectToBeCompared.RelationshipTypeName)
                    return false;

                if (this.EntityTypeId != objectToBeCompared.EntityTypeId)
                    return false;

                if (this.EntityTypeName != objectToBeCompared.EntityTypeName)
                    return false;

                if (this.ToEntityTypeId != objectToBeCompared.ToEntityTypeId)
                    return false;

                if (this.ToEntityTypeName != objectToBeCompared.ToEntityTypeName)
                    return false;

                if (this.MinRelationships != objectToBeCompared.MinRelationships)
                    return false;

                if (this.MaxRelationships != objectToBeCompared.MaxRelationships)
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
            return base.GetHashCode() ^ this.OrganizationId.GetHashCode() ^ this.OrganizationName.GetHashCode() ^ this.ContainerId.GetHashCode() ^ this.ContainerName.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.RelationshipTypeName.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.ToEntityTypeId.GetHashCode() ^ this.ToEntityTypeName.GetHashCode() ^ this.MinRelationships.GetHashCode() ^ this.MaxRelationships.GetHashCode();
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