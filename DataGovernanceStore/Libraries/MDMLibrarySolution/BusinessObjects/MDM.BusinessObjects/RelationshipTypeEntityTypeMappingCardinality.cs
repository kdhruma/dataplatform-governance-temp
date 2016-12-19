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
    /// Specifies the mapping object of RelationshipType EntityType Mapping Cardinality
    /// </summary>
    [DataContract]
    public class RelationshipTypeEntityTypeMappingCardinality : RelationshipTypeEntityTypeMapping, IRelationshipTypeEntityTypeMappingCardinality, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field for the To RelationshipTypeEntityType Id
        /// </summary>
        private Int32 _relationshipTypeEntityTypeId = -1;

        /// <summary>
        /// Field for the To EntityType Id
        /// </summary>
        private Int32 _toEntityTypeId = -1;

        /// <summary>
        /// Field for the To EntityType Name
        /// </summary>
        private String _toEntityTypeName = String.Empty;

        /// <summary>
        /// Field for the To EntityType Long Name
        /// </summary>
        private String _toEntityTypeLongName = String.Empty;

        /// <summary>
        /// Field for the Min Relationships
        /// </summary>
        private Int32 _minRelationships = 0;

        /// <summary>
        /// Field for the Max Relationships
        /// </summary>
        private Int32 _maxRelationships = 0;

        /// <summary>
        /// Field for the ExternalId
        /// </summary>
        private String _externalId = String.Empty;

        /// <summary>
        /// Field for the Original RelationshipType EntityType Mapping Cardinality
        /// </summary>
        private RelationshipTypeEntityTypeMappingCardinality _originalRelationshipTypeEntityTypeMappingCardinality = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipTypeEntityTypeMappingCardinality()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of RelationshipType EntityType Mapping Cardinality</param>
        public RelationshipTypeEntityTypeMappingCardinality(Int32 id) : base(id)
        {
        }

        /// <summary>
        /// Constructor with RelationshipType Id, Entity Type Id and To Entity Type Id as input parameters
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationship id</param>
        /// <param name="entityTypeId">Indicates entity type id</param>
        /// <param name="toEntityTypeId">Indicates related entity type id</param>
        public RelationshipTypeEntityTypeMappingCardinality(Int32 relationshipTypeId, Int32 entityTypeId, Int32 toEntityTypeId)
        {
            this.RelationshipTypeId = relationshipTypeId;
            this.EntityTypeId = entityTypeId;
            this._toEntityTypeId = toEntityTypeId;
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
                return "RelationshipTypeEntityTypeMappingCardinality";
            }
        }

        /// <summary>
        /// Property defining the To RelationshipTypeEntityType Id
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeEntityTypeId
        {
            get
            {
                return _relationshipTypeEntityTypeId;
            }
            set
            {
                _relationshipTypeEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property defining the To EntityType Id
        /// </summary>
        [DataMember]
        public Int32 ToEntityTypeId
        {
            get
            {
                return _toEntityTypeId;
            }
            set
            {
                _toEntityTypeId = value;
            }
        }

        /// <summary>
        /// Property defining the To EntityType Name
        /// </summary>
        [DataMember]
        public String ToEntityTypeName
        {
            get
            {
                return _toEntityTypeName;
            }
            set
            {
                _toEntityTypeName = value;
            }
        }

        /// <summary>
        /// Property defining the To EntityType Long Name
        /// </summary>
        [DataMember]
        public String ToEntityTypeLongName
        {
            get
            {
                return _toEntityTypeLongName;
            }
            set
            {
                _toEntityTypeLongName = value;
            }
        }

        /// <summary>
        /// Property defining the Min Relationships
        /// </summary>
        [DataMember]
        public Int32 MinRelationships
        {
            get
            {
                return _minRelationships;
            }
            set
            {
                _minRelationships = value;
            }
        }

        /// <summary>
        /// Property defining the Max Relationships
        /// </summary>
        [DataMember]
        public Int32 MaxRelationships
        {
            get
            {
                return _maxRelationships;
            }
            set
            {
                _maxRelationships = value;
            }
        }

        /// <summary>
        /// Property denoting Original RelationshipType EntityType Mapping Cardinality.
        /// </summary>
        public RelationshipTypeEntityTypeMappingCardinality OriginalRelationshipTypeEntityTypeMappingCardinality
        {
            get { return _originalRelationshipTypeEntityTypeMappingCardinality; }
            set { _originalRelationshipTypeEntityTypeMappingCardinality = value; }
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
                return MDM.Core.ObjectType.RelationshipTypeEntityTypeMappingCardinality;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region IRelationshipTypeEntityTypeMappingCardinality

        /// <summary>
        /// Get Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </summary>
        /// <returns>
        /// Xml representation of RelationshipType EntityType Mapping Cardinality
        /// </returns>
        public new String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            //RelationshipTypeEntityTypeMappingCardinality node start
            xmlWriter.WriteStartElement("RelationshipEntityTypeCardinality");
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("EntityTypeId", this.EntityTypeId.ToString());
            xmlWriter.WriteAttributeString("EntityTypeName", this.EntityTypeName);
            xmlWriter.WriteAttributeString("RelationshipTypeId", this.RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeName", this.RelationshipTypeName);

            xmlWriter.WriteStartElement("Cardinality");
            xmlWriter.WriteAttributeString("RelationshipTypeEntityTypeCardinalityId", this.Id.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeEntityTypeId", this.RelationshipTypeEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeId", this.ToEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("ToEntityTypeName", this.ToEntityTypeName);
            xmlWriter.WriteAttributeString("MinRelationships", this.MinRelationships.ToString());
            xmlWriter.WriteAttributeString("MinRelationships", this.MinRelationships.ToString());
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
        /// Clone RelationshipTypeEntityTypeMappingCardinality object
        /// </summary>
        /// <returns>
        /// Cloned copy of IRelationshipTypeEntityTypeMappingCardinality object.
        /// </returns>
        public new IRelationshipTypeEntityTypeMappingCardinality Clone()
        {
            RelationshipTypeEntityTypeMappingCardinality clonedRelationshipTypeEntityTypeMappingCardinality = new RelationshipTypeEntityTypeMappingCardinality();

            clonedRelationshipTypeEntityTypeMappingCardinality.Id = this.Id;
            clonedRelationshipTypeEntityTypeMappingCardinality.Name = this.Name;
            clonedRelationshipTypeEntityTypeMappingCardinality.LongName = this.LongName;
            clonedRelationshipTypeEntityTypeMappingCardinality.Locale = this.Locale;
            clonedRelationshipTypeEntityTypeMappingCardinality.Action = this.Action;
            clonedRelationshipTypeEntityTypeMappingCardinality.AuditRefId = this.AuditRefId;
            clonedRelationshipTypeEntityTypeMappingCardinality.ExtendedProperties = this.ExtendedProperties;

            clonedRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeEntityTypeId = this.RelationshipTypeEntityTypeId;
            clonedRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeId = this.RelationshipTypeId;
            clonedRelationshipTypeEntityTypeMappingCardinality.RelationshipTypeName = this.RelationshipTypeName;
            clonedRelationshipTypeEntityTypeMappingCardinality.EntityTypeId = this.EntityTypeId;
            clonedRelationshipTypeEntityTypeMappingCardinality.EntityTypeName = this.EntityTypeName;
            clonedRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeId = this.ToEntityTypeId;
            clonedRelationshipTypeEntityTypeMappingCardinality.ToEntityTypeName = this.ToEntityTypeName;
            clonedRelationshipTypeEntityTypeMappingCardinality.MinRelationships = this.MinRelationships;
            clonedRelationshipTypeEntityTypeMappingCardinality.MaxRelationships = this.MaxRelationships;

            return clonedRelationshipTypeEntityTypeMappingCardinality;
        }

        /// <summary>
        /// Delta Merge of RelationshipTypeEntityTypeMappingCardinality
        /// </summary>
        /// <param name="deltaRelationshipTypeEntityTypeMappingCardinality">RelationshipTypeEntityTypeMappingCardinality that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipTypeEntityTypeMappingCardinality instance</returns>
        public IRelationshipTypeEntityTypeMappingCardinality MergeDelta(IRelationshipTypeEntityTypeMappingCardinality deltaRelationshipTypeEntityTypeMappingCardinality, ICallerContext iCallerContext, bool returnClonedObject = true)
        {
            IRelationshipTypeEntityTypeMappingCardinality mergedRelationshipTypeEntityTypeMappingCardinality = (returnClonedObject == true) ? deltaRelationshipTypeEntityTypeMappingCardinality.Clone() : deltaRelationshipTypeEntityTypeMappingCardinality;

            mergedRelationshipTypeEntityTypeMappingCardinality.Action = (mergedRelationshipTypeEntityTypeMappingCardinality.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedRelationshipTypeEntityTypeMappingCardinality;
        }
       
        #endregion
        
        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">RelationshipTypeEntityTypeMappingCardinality object which needs to be compared.</param>
        /// <returns>Result of the comparison in Boolean.</returns>
        public override Boolean Equals(object obj)
        {
            if (obj is RelationshipTypeEntityTypeMappingCardinality)
            {
                RelationshipTypeEntityTypeMappingCardinality objectToBeCompared = obj as RelationshipTypeEntityTypeMappingCardinality;

                if (!base.Equals(objectToBeCompared))
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
            return base.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.RelationshipTypeName.GetHashCode() ^ this.EntityTypeId.GetHashCode() 
                    ^ this.EntityTypeName.GetHashCode() ^ this.ToEntityTypeId.GetHashCode() ^ this.ToEntityTypeName.GetHashCode() ^ this.MinRelationships.GetHashCode() 
                    ^ this.MaxRelationships.GetHashCode();
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

        #endregion
    }
}