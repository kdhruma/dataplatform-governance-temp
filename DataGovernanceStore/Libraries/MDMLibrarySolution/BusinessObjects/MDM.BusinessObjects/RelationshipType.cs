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
    /// Specifies the RelationshipType
    /// </summary>
    [DataContract]
    [ProtoContract]
    public class RelationshipType : MDMObject, IRelationshipType , IDataModelObject
    {
        #region Fields

        /// <summary>
        /// Field denoting validation required for relationship type.
        /// </summary>
        private Boolean _validationRequired = false;

        /// <summary>
        /// Field denoting show valid flag for relationship type.
        /// </summary>
        private Boolean _showValidFlag = false;

        /// <summary>
        /// Field denoting show read only
        /// </summary>
        private Boolean _readOnly = false;

        /// <summary>
        /// Field denoting show drill down
        /// </summary>
        private Boolean _drillDown = false;

        /// <summary>
        /// Field denoting show is default for relationship type.
        /// </summary>
        private String _isDefault = String.Empty;

        /// <summary>
        /// Property to identify if the state of the related entity impacts the state of the source entity
        /// </summary>
        private Boolean _enforceRelatedEntityStateOnSourceEntity = false;

        /// <summary>
        /// Property to identify if the promote of the current entity requires the related entity to be promoted already
        /// </summary>
        private Boolean _checkRelatedEntityPromoteStatusOnPromote = false;

        /// <summary>
        /// Field Denoting the original relationship type
        /// </summary>
        private RelationshipType _originalRelationshipType = null;

        #region IDataModelObject Fields

        /// <summary>
        /// Field denoting Organization key External Id from external source.
        /// </summary>
        private String _externalId = String.Empty;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public RelationshipType()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Id of a Relationship Type</param>
        public RelationshipType(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Relationship Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Id of a Relationship Type</param>
        /// <param name="name">Indicates the Name of a Relationship Type</param>
        /// <param name="longName">Indicates the Description of a Relationship Type</param>
        public RelationshipType(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, Description, Enforce Related Entity State On Source Entity flag and Check Related Entity Promote Status On Promote flag of a Relationship Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Id of a Relationship Type</param>
        /// <param name="name">Indicates the Name of a Relationship Type</param>
        /// <param name="longName">Indicates the Description of a Relationship Type</param>
        /// <param name="enforceRelatedEntityStateOnSourceEntity">This parameter is identifying if the state of the related entity impacts the state of the source entity </param>
        /// <param name="checkRelatedEntityPromoteStatusOnPromote">This parameter is identifying if the promote of the current entity requires the related entity to be promoted already</param>
        public RelationshipType(Int32 id, String name, String longName, Boolean enforceRelatedEntityStateOnSourceEntity, Boolean checkRelatedEntityPromoteStatusOnPromote)
            : base(id, name, longName)
        {
            this._enforceRelatedEntityStateOnSourceEntity = enforceRelatedEntityStateOnSourceEntity;
            this._checkRelatedEntityPromoteStatusOnPromote = checkRelatedEntityPromoteStatusOnPromote;
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Relationship Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Id of a Relationship Type</param>
        /// <param name="name">Indicates the Name of a Relationship Type</param>
        /// <param name="longName">Indicates the LongName of a Relationship Type</param>
        /// <param name="locale">Indicates the Locale of a Relationship Type</param>
        public RelationshipType(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
         
        }

        /// <summary>
        /// Constructor with Id, Name, LongName, ValidationRequired, showValidFlag, readOnly, drillDown, isDefault, enforceRelatedEntityStateOnSourceEntity and checkRelatedEntityPromoteStatusOnPromote
        /// </summary>
        /// <param name="id">This parameter is specifying id.</param>
        /// <param name="name">This parameter is specifying short name.</param>
        /// <param name="longName">This parameter is specifying long name.</param>
        /// <param name="validationRequired">This parameter is specifying validation required.</param>
        /// <param name="showValidFlag">This parameter is specifying show valid flag.</param>
        /// <param name="readOnly">This parameter is specifying read only or not.</param>
        /// <param name="drillDown">This parameter is specifying drill down.</param>
        /// <param name="isDefault">This parameter is specifying IsDefault or not.</param>
        /// <param name="enforceRelatedEntityStateOnSourceEntity">This parameter is identifying if the state of the related entity impacts the state of the source entity </param>
        /// <param name="checkRelatedEntityPromoteStatusOnPromote">This parameter is identifying if the promote of the current entity requires the related entity to be promoted already</param>
        public RelationshipType(Int32 id, String name, String longName, Boolean validationRequired, Boolean showValidFlag, Boolean readOnly, Boolean drillDown, String isDefault, Boolean enforceRelatedEntityStateOnSourceEntity, Boolean checkRelatedEntityPromoteStatusOnPromote)
            : base(id,name,longName)
        {
            this._validationRequired = validationRequired;
            this._showValidFlag = showValidFlag;
            this._readOnly = readOnly;
            this._drillDown = drillDown;
            this._isDefault = isDefault;
            this._enforceRelatedEntityStateOnSourceEntity = enforceRelatedEntityStateOnSourceEntity;
            this._checkRelatedEntityPromoteStatusOnPromote = checkRelatedEntityPromoteStatusOnPromote;
        }
        
         /// <summary>
        /// Constructor with XML having values of object. Populate current object using XML
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///         &lt;RelationshipType Id="2" 
        ///             Name="Kit Code Products" 
        ///             LongName="Kit Code Products" 
        ///             ValidationRequired="1" 
        ///             ShowValidFlag="0" 
        ///             ReadOnly="0" 
        ///             DrillDown="0" 
        ///             IsDefault="1" /&gt;
        /// </para>
        /// </example>
        public RelationshipType(String valuesAsXml)
        {
            LoadRelationshipTypeDetail(valuesAsXml);
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
                return "RelationshipType";
            }
        }

        /// <summary>
        /// Property denoting Validation required
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public Boolean ValidationRequired
        {
            get
            {
                return this._validationRequired;
            }
            set
            {
                this._validationRequired = value;
            }
        }

        /// <summary>
        /// Property denoting Show valid flag
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public Boolean ShowValidFlag
        {
            get
            {
                return this._showValidFlag;
            }
            set
            {
                this._showValidFlag = value;
            }
        }

        /// <summary>
        /// Property denoting read only
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Boolean ReadOnly
        {
            get
            {
                return this._readOnly;
            }
            set
            {
                this._readOnly = value;
            }
        }

        /// <summary>
        /// Property denoting drill down.
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public Boolean DrillDown
        {
            get
            {
                return this._drillDown;
            }
            set
            {
                this._drillDown = value;
            }
        }

        /// <summary>
        /// Property denoting is default for relationship type
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String IsDefault
        {
            get
            {
                return this._isDefault;
            }
            set
            {
                this._isDefault = value;
            }
        }

        /// <summary>
        /// Property to identify if the state of the related entity impacts the state of the source entity
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public Boolean EnforceRelatedEntityStateOnSourceEntity
        {
            get
            {
                return _enforceRelatedEntityStateOnSourceEntity;
            }
            set
            {
                _enforceRelatedEntityStateOnSourceEntity = value;
            }
        }

        /// <summary>
        /// Property to identify if the promote of the current entity requires the related entity to be promoted already
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public Boolean CheckRelatedEntityPromoteStatusOnPromote
        {
            get
            {
                return _checkRelatedEntityPromoteStatusOnPromote;
            }
            set
            {
                _checkRelatedEntityPromoteStatusOnPromote = value;
            }
        }

        /// <summary>
        /// Property denoting the original relationship type
        /// </summary>
        public RelationshipType OriginalRelationshipType
        {
            get
            {
                return _originalRelationshipType;
            }
            set
            {
                this._originalRelationshipType = value;
            }
        }

        #region IDataModelObject Properties

        /// <summary>
        /// Property denoting a ObjectType for an dataModelObject object
        /// </summary>
        public ObjectType DataModelObjectType
        {
            get
            {
                return MDM.Core.ObjectType.RelationshipType;
            }
        }

        /// <summary>
        /// Property denoting the external id for an dataModelObject object
        /// </summary>
        public String ExternalId
        {
            get
            {
                return _externalId;
            }
            set
            {
                _externalId = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clone relationship type object
        /// </summary>
        /// <returns>cloned copy of relationship type object.</returns>
        public IRelationshipType Clone()
        {
            RelationshipType clonedRelationshipType = new RelationshipType();

            clonedRelationshipType.Id = this.Id;
            clonedRelationshipType.Name = this.Name;
            clonedRelationshipType.LongName = this.LongName;
            clonedRelationshipType.Locale = this.Locale;
            clonedRelationshipType.Action = this.Action;
            clonedRelationshipType.AuditRefId = this.AuditRefId;
            clonedRelationshipType.ExtendedProperties = this.ExtendedProperties;
            clonedRelationshipType.EnforceRelatedEntityStateOnSourceEntity = this.EnforceRelatedEntityStateOnSourceEntity;
            clonedRelationshipType.CheckRelatedEntityPromoteStatusOnPromote = this.CheckRelatedEntityPromoteStatusOnPromote;

            clonedRelationshipType._validationRequired = this._validationRequired;
            clonedRelationshipType._showValidFlag = this._showValidFlag;
            clonedRelationshipType._readOnly = this._readOnly;
            clonedRelationshipType._drillDown = this._drillDown;
            clonedRelationshipType._isDefault = this._isDefault;

            return clonedRelationshipType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (base.Equals(obj))
            {
                if (obj is RelationshipType)
                {
                    RelationshipType objectToBeCompared = obj as RelationshipType;

                    if (this.ObjectType != objectToBeCompared.ObjectType)
                        return false;

                    if (this.ValidationRequired != objectToBeCompared.ValidationRequired)
                        return false;

                    if (this.ShowValidFlag != objectToBeCompared.ShowValidFlag)
                        return false;

                    if (this.ReadOnly != objectToBeCompared.ReadOnly)
                        return false;

                    if (this.DrillDown != objectToBeCompared.DrillDown)
                        return false;

                    if (this.IsDefault != objectToBeCompared.IsDefault)
                        return false;

                    if (this.CheckRelatedEntityPromoteStatusOnPromote != objectToBeCompared.CheckRelatedEntityPromoteStatusOnPromote)
                        return false;

                    if (this.EnforceRelatedEntityStateOnSourceEntity != objectToBeCompared.EnforceRelatedEntityStateOnSourceEntity)
                        return false;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Get XML representation of Relationship Type object
        /// </summary>
        /// <returns>XML representation of Relationship Type object</returns>
        public override String ToXml()
        {
            String relationshipTypeXml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            xmlWriter.WriteStartElement("RelationshipType");

            #region Write relationship properties

            xmlWriter.WriteAttributeString("Id",this.Id.ToString());
            xmlWriter.WriteAttributeString("Name", this.Name);
            xmlWriter.WriteAttributeString("LongName", this.LongName);
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ValidationRequired", this.ValidationRequired.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ShowValidFlag",this.ShowValidFlag.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("ReadOnly",this.ReadOnly.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("DrillDown", this.DrillDown.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("IsDefault", this.IsDefault.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("EnforceRelatedEntityStateOnSourceEntity", this.EnforceRelatedEntityStateOnSourceEntity.ToString().ToLowerInvariant());
            xmlWriter.WriteAttributeString("CheckRelatedEntityPromoteStatusOnPromote", this.CheckRelatedEntityPromoteStatusOnPromote.ToString().ToLowerInvariant());

            #endregion

            //Relationship Type node end
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();

            //get the actual XML
            relationshipTypeXml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return relationshipTypeXml;
        }

        /// <summary>
        /// Get XML representation of Relationship Type object
        /// </summary>
        /// <returns>XML representation of Relationship Type object</returns>
        public override String ToXml(ObjectSerialization serialization)
        {
            return this.ToXml();
        }

        /// <summary>
        /// Delta Merge of relationship type
        /// </summary>
        /// <param name="deltaRelationship">Relationship Type that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged relationship type instance</returns>
        public IRelationshipType MergeDelta(IRelationshipType deltaRelationship, ICallerContext iCallerContext, Boolean returnClonedObject = true)
        {
            IRelationshipType mergedRelaitonshipType = (returnClonedObject == true) ? deltaRelationship.Clone() : deltaRelationship; 

            mergedRelaitonshipType.Action = (mergedRelaitonshipType.Equals(this)) ? ObjectAction.Read : ObjectAction.Update;

            return mergedRelaitonshipType;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetRelationshipType">The Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to determine whether id based comparison is true or not</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(RelationshipType subsetRelationshipType, Boolean compareIds = false)
        {
            if (subsetRelationshipType != null)
            {
                if (base.IsSuperSetOf(subsetRelationshipType, compareIds))
                {
                    if (this.ObjectType != subsetRelationshipType.ObjectType)
                    {
                        return false;
                    }

                    if (this.ValidationRequired != subsetRelationshipType.ValidationRequired)
                    {
                        return false;
                    }

                    if (this.ShowValidFlag != subsetRelationshipType.ShowValidFlag)
                    {
                        return false;
                    }

                    if (this.ReadOnly != subsetRelationshipType.ReadOnly)
                    {
                        return false;
                    }

                    if (this.DrillDown != subsetRelationshipType.DrillDown)
                    {
                        return false;
                    }

                    if (this.EnforceRelatedEntityStateOnSourceEntity != subsetRelationshipType.EnforceRelatedEntityStateOnSourceEntity)
                    {
                        return false;
                    }

                    if (this.CheckRelatedEntityPromoteStatusOnPromote != subsetRelationshipType.CheckRelatedEntityPromoteStatusOnPromote)
                    {
                        return false;
                    }

                    if (String.Compare(this.IsDefault, subsetRelationshipType.IsDefault) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

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

        #endregion

        #region Private Methods

        /// <summary>
        /// Load relationship type object from XML.
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        /// <example>
        ///     Sample XML:
        ///     <para>
        ///     <![CDATA[
        ///         <RelationshipType
        ///             Id="2" 
        ///             Name="Kit Code Products" 
        ///             LongName="Kit Code Products" 
        ///             ValidationRequired="1" 
        ///             ShowValidFlag="0"
        ///             ReadOnly="0" 
        ///             DrillDown="0" 
        ///             IsDefault="1">
        ///         </RelationshipType>
        ///     ]]>    
        ///     </para>
        /// </example>
        private void LoadRelationshipTypeDetail(String valuesAsXml)
        {
            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "RelationshipType" && reader.IsStartElement())
                        {
                            //Read relationship type metadata
                            #region Read Relationship Type Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = reader.ReadContentAsInt();
                                }
                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("LongName"))
                                {
                                    this.LongName = reader.ReadContentAsString();
                                }
                                if (reader.MoveToAttribute("Action"))
                                {
                                    ObjectAction action;
                                    if (Enum.TryParse(reader.ReadContentAsString(), out action))
                                    {
                                        this.Action = action;
                                    }
                                }
                                if (reader.MoveToAttribute("ValidationRequired"))
                                {
                                    this.ValidationRequired = reader.ReadContentAsBoolean();
                                }
                                if (reader.MoveToAttribute("ShowValidFlag"))
                                {
                                    this.ShowValidFlag = reader.ReadContentAsBoolean();
                                }
                                if (reader.MoveToAttribute("ReadOnly"))
                                {
                                    this.ReadOnly = reader.ReadContentAsBoolean();
                                }
                                if (reader.MoveToAttribute("DrillDown"))
                                {
                                    this.DrillDown = reader.ReadContentAsBoolean();
                                }
                                if (reader.MoveToAttribute("IsDefault"))
                                {
                                    this.IsDefault = reader.ReadContentAsString();
                            }
                                if (reader.MoveToAttribute("CheckRelatedEntityPromoteStatusOnPromote"))
                                {
                                    this.CheckRelatedEntityPromoteStatusOnPromote = reader.ReadContentAsBoolean();
                                }
                                if (reader.MoveToAttribute("EnforceRelatedEntityStateOnSourceEntity"))
                                {
                                    this.EnforceRelatedEntityStateOnSourceEntity = reader.ReadContentAsBoolean();
                                }
                            }

                            #endregion
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