using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies the mapping object for RelationshipType Attribute mapping
    /// </summary>
    [DataContract]
    public class RelationshipTypeEntityTypeMapping : MDMObject, IRelationshipTypeEntityTypeMapping, IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting the RelationshipType Id
        /// </summary>
        private Int32 _relationshipTypeId = -1;

        /// <summary>
        /// Field denoting the short name of relationshipType
        /// </summary>
        private String _relationshipTypeName = String.Empty;

        /// <summary>
        /// Field denoting the long name of relationshipType
        /// </summary>
        private String _relationshipTypeLongName = String.Empty;
        
        /// <summary>
        /// Field denoting the entity type id
        /// </summary>
        private Int32 _entityTypeId = -1;

        /// <summary>
        /// Field denoting the short name of entity type 
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting the long name of entity type 
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Field denoting the DrillDown property of mapping
        /// </summary>
        private Boolean _drillDown = false;

        /// <summary>
        /// Field denoting the IsDefaultRelation property of mapping
        /// </summary>
        private Boolean _isDefaultRelation = false;

        /// <summary>
        /// Field denoting the Excludable property of mapping
        /// </summary>
        private Boolean _excludable = false;

        /// <summary>
        /// Field denoting the ReadOnly property of mapping
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field denoting the ValidationRequired property of mapping
        /// </summary>
        private Boolean _validationRequired = false;

        /// <summary>
        /// Field denoting the ShowValidFlagInGrid property of mapping
        /// </summary>
        private Boolean _showValidFlagInGrid = false;

        /// <summary>
        /// Field denoting the OriginalRelationshipTypeEntityTypeMapping property of mapping
        /// </summary>
        private RelationshipTypeEntityTypeMapping _originalRelationshipTypeEntityTypeMapping = null;

        /// <summary>
        /// Field denoting the ExternalId property of mapping
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipTypeEntityTypeMapping()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of RelationshipType - EntityType Mapping</param>
        public RelationshipTypeEntityTypeMapping(Int32 id) :
            base(id)
        {

        }

        /// <summary>
        /// Constructor with String as input parameter
        /// </summary>
        /// <param name="valueAsXml">Indicates the Identity of RelationshipType - EntityType Mapping</param>
        public RelationshipTypeEntityTypeMapping(String valueAsXml)
            : this()
        {
            LoadRelationshipTypeEntityTypeMappingFromXml(valueAsXml);
        }

        /// <summary>
        /// Constructor with RelationshipType Id and Entity Type Id as input parameters
        /// </summary>
        /// <param name="relationshipTypeId">Indicates relationship id</param>
        /// <param name="entityTypeId">Indicates entity type id</param>
        public RelationshipTypeEntityTypeMapping(Int32 relationshipTypeId, Int32 entityTypeId)
        {
            this._relationshipTypeId = relationshipTypeId;
            this._entityTypeId = entityTypeId;
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
                return "RelationshipTypeEntityTypeMapping";
            }
        }

        /// <summary>
        /// Property denoting mapped RelationshipType Id
        /// </summary>
        [DataMember]
        public Int32 RelationshipTypeId
        {
            get { return _relationshipTypeId; }
            set { _relationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting the mapped RelationshipType name
        /// </summary>
        [DataMember]
        public String RelationshipTypeName
        {
            get { return _relationshipTypeName; }
            set { _relationshipTypeName = value; }
        }

        /// <summary>
        /// Property denoting the mapped RelationshipType long name
        /// </summary>
        [DataMember]
        public String RelationshipTypeLongName
        {
            get { return _relationshipTypeLongName; }
            set { _relationshipTypeLongName = value; }
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
        /// Property denoting the mapped Entity Type name
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
            get { return _entityTypeLongName ; }
            set { _entityTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting the DrillDown for this Mapping
        /// </summary>
        [DataMember]
        public Boolean DrillDown
        {
            get { return _drillDown; }
            set { _drillDown = value; }
        }

        /// <summary>
        /// Property denoting the IsDefaultRelation for this Mapping
        /// </summary>
        [DataMember]
        public Boolean IsDefaultRelation
        {
            get { return _isDefaultRelation; }
            set { _isDefaultRelation = value; }
        }

        /// <summary>
        /// Property denoting the Excludable for this Mapping
        /// </summary>
        [DataMember]
        public Boolean Excludable
        {
            get { return _excludable; }
            set { _excludable = value; }
        }

        /// <summary>
        /// Indicates the Read Only for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        /// <summary>
        /// Property denoting the ValidationRequired for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ValidationRequired
        {
            get { return _validationRequired; }
            set { _validationRequired = value; }
        }

        /// <summary>
        /// Property denoting the ShowValidFlagInGrid for this Mapping
        /// </summary>
        [DataMember]
        public Boolean ShowValidFlagInGrid
        {
            get { return _showValidFlagInGrid; }
            set { _showValidFlagInGrid = value; }
        }

        /// <summary>
        /// Property denoting the OriginalRelationshipTypeEntityTypeMapping for this Mapping
        /// </summary>
        public RelationshipTypeEntityTypeMapping OriginalRelationshipTypeEntityTypeMapping
        {
            get{ return _originalRelationshipTypeEntityTypeMapping; }
            set { _originalRelationshipTypeEntityTypeMapping = value; }
        }

        #region IDataModelObject

        /// <summary>
        /// Property denoting the ExternalId for this Mapping
        /// </summary>
        public String ExternalId
        {
            get { return _externalId; }
            set { _externalId = value; }
        }

        /// <summary>
        /// Property denoting the DataModelObjectType for this Mapping
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.RelationshipTypeEntityTypeMapping;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region IRelationshipTypeEntityTypeMapping

        /// <summary>
        /// Get Xml representation of RelationshipTypeEntityType object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        public String ToXML()
        {
            String xml = string.Empty;

            xml = "<RelationshipTypeEntityType Id=\"{0}\" RelationshipTypeId=\"{1}\" EntityTypeId = \"{2}\" DrillDown = \"{3}\" IsDefaultRelation=\"{4}\" Excludable=\"{5}\" ReadOnly=\"{6}\" ValidationRequired=\"{7}\" ShowValidFlagInGrid=\"{8}\" Action=\"{9}\" />    ";

            string retXML = string.Format(xml, this.Id, this.RelationshipTypeId, this.EntityTypeId, this.DrillDown, this.IsDefaultRelation, this.Excludable, this.ReadOnly, this.ValidationRequired, this.ShowValidFlagInGrid, this.Action);

            return retXML;
        }

        /// <summary>
        /// Clone RelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of IRelationshipTypeEntityTypeMapping object.</returns>
        public IRelationshipTypeEntityTypeMapping Clone()
        {
            RelationshipTypeEntityTypeMapping clonedRelationshipTypeEntityTypeMapping = new RelationshipTypeEntityTypeMapping();

            clonedRelationshipTypeEntityTypeMapping.Id = this.Id;
            clonedRelationshipTypeEntityTypeMapping.Name = this.Name;
            clonedRelationshipTypeEntityTypeMapping.LongName = this.LongName;
            clonedRelationshipTypeEntityTypeMapping.Locale = this.Locale;
            clonedRelationshipTypeEntityTypeMapping.Action = this.Action;
            clonedRelationshipTypeEntityTypeMapping.AuditRefId = this.AuditRefId;
            clonedRelationshipTypeEntityTypeMapping.ExtendedProperties = this.ExtendedProperties;

            clonedRelationshipTypeEntityTypeMapping.EntityTypeId = this.EntityTypeId;
            clonedRelationshipTypeEntityTypeMapping.EntityTypeName = this.EntityTypeName;
            clonedRelationshipTypeEntityTypeMapping.RelationshipTypeId = this.RelationshipTypeId;
            clonedRelationshipTypeEntityTypeMapping.RelationshipTypeName = this.RelationshipTypeName;
            clonedRelationshipTypeEntityTypeMapping.DrillDown = this.DrillDown;
            clonedRelationshipTypeEntityTypeMapping.Excludable = this.Excludable;
            clonedRelationshipTypeEntityTypeMapping.IsDefaultRelation = this.IsDefaultRelation;
            clonedRelationshipTypeEntityTypeMapping.ReadOnly = this.ReadOnly;
            clonedRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid = this.ShowValidFlagInGrid;
            clonedRelationshipTypeEntityTypeMapping.ValidationRequired = this.ValidationRequired;

            return clonedRelationshipTypeEntityTypeMapping;
        }

        /// <summary>
        /// Delta Merge of RelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="deltaRelationshipTypeEntityTypeMapping">RelationshipTypeEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipTypeEntityTypeMapping instance</returns>
        public IRelationshipTypeEntityTypeMapping MergeDelta(IRelationshipTypeEntityTypeMapping deltaRelationshipTypeEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IRelationshipTypeEntityTypeMapping mergedRelationshipTypeEntityTypeMapping = (returnClonedObject == true) ? deltaRelationshipTypeEntityTypeMapping.Clone() : deltaRelationshipTypeEntityTypeMapping;

            mergedRelationshipTypeEntityTypeMapping.Action = (mergedRelationshipTypeEntityTypeMapping.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedRelationshipTypeEntityTypeMapping;

        }

        /// <summary>
        /// Compare relationship type entity type mapping with current relationship type entity type mapping .
        /// This method will compare relationship type entity type mapping .
        /// </summary>
        /// <param name="subsetRelationshipTypeEntityTypeMapping">
        /// Indicates relationship type entity type mapping to be compared with current relationship type entity type mapping.
        /// </param>
        /// <param name="compareIds">Indicates flag to determine whether id based comparison shold be performed or not.</param>
        /// <returns>Returns true : if both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(RelationshipTypeEntityTypeMapping subsetRelationshipTypeEntityTypeMapping, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.Id != subsetRelationshipTypeEntityTypeMapping.Id)
                    return false;

                if (this.ReferenceId != subsetRelationshipTypeEntityTypeMapping.ReferenceId)
                    return false;

                if (this.RelationshipTypeId != subsetRelationshipTypeEntityTypeMapping.RelationshipTypeId)
                    return false;

                if (this.EntityTypeId != subsetRelationshipTypeEntityTypeMapping.EntityTypeId)
                    return false;
            }

            if (this.RelationshipTypeName != subsetRelationshipTypeEntityTypeMapping.RelationshipTypeName)
                return false;
            
            if (this.EntityTypeName != subsetRelationshipTypeEntityTypeMapping.EntityTypeName)
                return false;

            if (this.DrillDown != subsetRelationshipTypeEntityTypeMapping.DrillDown)
                return false;

            if (this.Excludable != subsetRelationshipTypeEntityTypeMapping.Excludable)
                return false;

            if (this.IsDefaultRelation != subsetRelationshipTypeEntityTypeMapping.IsDefaultRelation)
                return false;

            if (this.ValidationRequired != subsetRelationshipTypeEntityTypeMapping.ValidationRequired)
                return false;

            if (this.ReadOnly != subsetRelationshipTypeEntityTypeMapping.ReadOnly)
                return false;

            if (this.ShowValidFlagInGrid != subsetRelationshipTypeEntityTypeMapping.ShowValidFlagInGrid)
                return false;

            if (this.Action != subsetRelationshipTypeEntityTypeMapping.Action)
                return false;

            return true;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">RelationshipTypeEntityTypeMapping object which needs to be compared.</param>
        /// <returns>Result of the comparison in boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is RelationshipTypeEntityTypeMapping)
            {
                RelationshipTypeEntityTypeMapping objectToBeCompared = obj as RelationshipTypeEntityTypeMapping;

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

                if (this.DrillDown != objectToBeCompared.DrillDown)
                    return false;

                if (this.IsDefaultRelation != objectToBeCompared.IsDefaultRelation)
                    return false;

                if (this.Excludable != objectToBeCompared.Excludable)
                    return false;

                if (this.ReadOnly != objectToBeCompared.ReadOnly)
                    return false;

                if (this.ValidationRequired != objectToBeCompared.ValidationRequired)
                    return false;

                if (this.ShowValidFlagInGrid != objectToBeCompared.ShowValidFlagInGrid)
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
            return base.GetHashCode() ^ this.RelationshipTypeId.GetHashCode() ^ this.RelationshipTypeName.GetHashCode() ^ this.EntityTypeId.GetHashCode() ^ this.EntityTypeName.GetHashCode() ^ this.DrillDown.GetHashCode() ^ this.IsDefaultRelation.GetHashCode() ^ this.Excludable.GetHashCode() ^ this.ReadOnly.GetHashCode() ^ this.ValidationRequired.GetHashCode() ^ this.ShowValidFlagInGrid.GetHashCode();
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
        /// <param name="valuesAsXml">Indicates xml having values for current relationship type entity type mapping</param>
        private void LoadRelationshipTypeEntityTypeMappingFromXml(string valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipTypeEntityType")
                        {
                            #region Read RelationshipTypeEntityType

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("RelationshipTypeId"))
                                {
                                    this.RelationshipTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("RelationshipTypeName"))
                                {
                                    this.RelationshipTypeName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("EntityTypeId"))
                                {
                                    this.EntityTypeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), 0);
                                }
                                if (reader.MoveToAttribute("EntityTypeName"))
                                {
                                    this.EntityTypeName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("DrillDown"))
                                {
                                    this.DrillDown = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("IsDefaultRelation"))
                                {
                                    this.IsDefaultRelation = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("Excludable"))
                                {
                                    this.Excludable = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ValidationRequired"))
                                {
                                    this.ValidationRequired = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("ShowValidFlagInGrid"))
                                {
                                    this.ShowValidFlagInGrid = ValueTypeHelper.ConvertToBoolean(reader.ReadContentAsString());
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action = ObjectAction.Unknown;
                                    Enum.TryParse(reader.ReadContentAsString(), out action);
                                    this.Action = action;
                                }
                            }

                            #endregion Read RelationshipTypeEntityType
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
        
        #endregion Private Methods

        #endregion
    }
}