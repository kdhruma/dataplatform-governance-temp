using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Collections.ObjectModel;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies Entity State Validation   
    /// </summary>
    [DataContract]
    [ProtoContract]
    [KnownType(typeof(MDMObject))]
    public class EntityStateValidation : MDMObject, IEntityStateValidation
    {
        #region Fields

        /// <summary>
        /// Indicates unique identifier for System validation state attribute  
        /// </summary>
        private SystemAttributes _systemValidationStateAttribute;

        /// <summary>
        ///  Indicates Reason type
        /// </summary>
        private ReasonType _reasonType = ReasonType.NotSpecified;

        /// <summary>
        /// Indicates unique identifier for Entity  
        /// </summary>
        private Int64 _entityId = -1;

        /// <summary>
        /// Indicates name for Entity  
        /// </summary>
        private String _entityName = String.Empty;

        /// <summary>
        /// Indicates long name for Entity  
        /// </summary>
        private String _entityLongName = String.Empty;

        /// <summary>
        /// Indicates entity type long name of an entity  
        /// </summary>
        private String _entityTypeLongName = String.Empty;

        /// <summary>
        /// Indicates category long name of an entity  
        /// </summary>
        private String _categoryLongName = String.Empty;

        /// <summary>
        ///  Indicates Container identifier
        /// </summary>
        private Int32 _containerId = -1;

        /// <summary>
        /// Indicates container name for Entity  
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Indicates container long name for Entity  
        /// </summary>
        private String _containerLongName = String.Empty;

        /// <summary>
        /// Indicates unique identifier for relationship 
        /// </summary>
        private Int64 _relationshipId = -1;

        /// <summary>
        /// Indicates relationship type long name for Entity
        /// </summary>
        private String _relationshipTypeLongName = String.Empty;

        /// <summary>
        /// Indicates unique identifier for related entity 
        /// </summary>
        private Int64 _relatedEntityId = -1;

        /// <summary>
        /// Indicates name of related entity  
        /// </summary>
        private String _relatedEntityName = String.Empty;

        /// <summary>
        /// Indicates long name of related entity
        /// </summary>
        private String _relatedEntityLongName = String.Empty;

        /// <summary>
        ///  Indicates attribute model type
        /// </summary>
        private AttributeModelType _attributeModelType;

        /// <summary>
        /// Indicates attribute identifier
        /// </summary>
        private Int32 _attributeId = -1;

        /// <summary>
        /// Indicates attribute name
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Indicates attribute long name
        /// </summary>
        private String _attributeLongName = String.Empty;

        /// <summary>
        /// Indicates message code
        /// </summary>
        private String _messageCode = String.Empty;

        /// <summary>
        /// Indicates message parameter
        /// </summary>
        private Collection<Object> _messageParameters = null;

        /// <summary>
        /// Indicates unique identifier for rule map context  
        /// </summary>
        private Int32 _ruleMapContextId = -1;

        /// <summary>
        /// Indicates rule map context name
        /// </summary>
        private String _ruleMapContextName = String.Empty;

        /// <summary>
        /// Indicates id for rule
        /// </summary>
        private Int32 _ruleId = -1;

        /// <summary>
        /// Indicates rule name
        /// </summary>
        private String _ruleName = String.Empty;

        /// <summary>
        /// Indicates operation result type
        /// </summary>
        private OperationResultType _operationResultType;

        /// <summary>
        /// Indicates job id
        /// </summary>
        private Int32 _jobId = 0;

        /// <summary>
        /// Indicates time stamp indicating when error was recorded.
        /// </summary>
        private DateTime _auditTimeStamp = DateTime.MinValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public EntityStateValidation()
            : base()
        {
        }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml value</param>
        public EntityStateValidation(String valuesAsXml)
        {
            this.LoadEntityValidationState(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates unique identifier for System validation state attribute  
        /// </summary>
        [DataMember]
        [ProtoMember(1)]
        public SystemAttributes SystemValidationStateAttribute
        {
            get
            {
                return this._systemValidationStateAttribute;
            }
            set
            {
                this._systemValidationStateAttribute = value;
            }
        }

        /// <summary>
        ///  Indicates reason type
        /// </summary>
        [DataMember]
        [ProtoMember(2)]
        public ReasonType ReasonType
        {
            get
            {
                return this._reasonType;
            }
            set
            {
                this._reasonType = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for Entity  
        /// </summary>
        [DataMember]
        [ProtoMember(3)]
        public Int64 EntityId
        {
            get
            {
                return this._entityId;
            }
            set
            {
                this._entityId = value;
            }
        }

        /// <summary>
        /// Indicates name for Entity  
        /// </summary>
        [DataMember]
        [ProtoMember(4)]
        public String EntityName
        {
            get
            {
                return this._entityName;
            }
            set
            {
                this._entityName = value;
            }
        }

        /// <summary>
        /// Indicates long name for Entity  
        /// </summary>
        [DataMember]
        [ProtoMember(5)]
        public String EntityLongName
        {
            get
            {
                return this._entityLongName;
            }
            set
            {
                this._entityLongName = value;
            }
        }

        /// <summary>
        /// Indicates entity type long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(6)]
        public String EntityTypeLongName
        {
            get
            {
                return this._entityTypeLongName;
            }
            set
            {
                this._entityTypeLongName = value;
            }
        }

        /// <summary>
        /// Indicates category long name of an entity
        /// </summary>
        [DataMember]
        [ProtoMember(7)]
        public String CategoryLongName
        {
            get
            {
                return this._categoryLongName;
            }
            set
            {
                this._categoryLongName = value;
            }
        }
        
        /// <summary>
        /// Indicates unique identifier for container  
        /// </summary>
        [DataMember]
        [ProtoMember(8)]
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
        /// Indicates container name for Entity  
        /// </summary>
        [DataMember]
        [ProtoMember(9)]
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
        /// Indicates container long name for Entity  
        /// </summary>
        [DataMember]
        [ProtoMember(10)]
        public String ContainerLongName
        {
            get
            {
                return this._containerLongName;
            }
            set
            {
                this._containerLongName = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for relationship  
        /// </summary>
        [DataMember]
        [ProtoMember(11)]
        public Int64 RelationshipId
        {
            get
            {
                return this._relationshipId;
            }
            set
            {
                this._relationshipId = value;
            }
        }

        /// <summary>
        /// Indicates relationship type long name for Entity 
        /// </summary>
        [DataMember]
        [ProtoMember(12)]
        public String RelationshipTypeLongName
        {
            get
            {
                return this._relationshipTypeLongName;
            }
            set
            {
                this._relationshipTypeLongName = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for related entity   
        /// </summary>
        [DataMember]
        [ProtoMember(13)]
        public Int64 RelatedEntityId
        {
            get
            {
                return this._relatedEntityId;
            }
            set
            {
                this._relatedEntityId = value;
            }
        }

        /// <summary>
        /// Indicates name of related entity
        /// </summary>
        [DataMember]
        [ProtoMember(14)]
        public String RelatedEntityName
        {
            get
            {
                return this._relatedEntityName;
            }
            set
            {
                this._relatedEntityName = value;
            }
        }

        /// <summary>
        /// Indicates long name of related entity  
        /// </summary>
        [DataMember]
        [ProtoMember(15)]
        public String RelatedEntityLongName
        {
            get
            {
                return this._relatedEntityLongName;
            }
            set
            {
                this._relatedEntityLongName = value;
            }
        }

        /// <summary>
        /// Indicates attribute model type  
        /// </summary>
        [DataMember]
        [ProtoMember(16)]
        public AttributeModelType AttributeModelType
        {
            get
            {
                return this._attributeModelType;
            }
            set
            {
                this._attributeModelType = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for attribute   
        /// </summary>
        [DataMember]
        [ProtoMember(17)]
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

        /// <summary>
        /// Indicates name of an attribute
        /// </summary>
        [DataMember]
        [ProtoMember(18)]
        public String AttributeName
        {
            get
            {
                return this._attributeName;
            }
            set
            {
                this._attributeName = value;
            }
        }

        /// <summary>
        /// Indicates long name of an attribute
        /// </summary>
        [DataMember]
        [ProtoMember(19)]
        public String AttributeLongName
        {
            get
            {
                return this._attributeLongName;
            }
            set
            {
                this._attributeLongName = value;
            }
        }

        /// <summary>
        /// Indicates message code for error/ warning / information 
        /// </summary>
        [DataMember]
        [ProtoMember(20)]
        public String MessageCode
        {
            get
            {
                return this._messageCode;
            }
            set
            {
                this._messageCode = value;
            }
        }

        /// <summary>
        /// Indicates message parameter with #@# string separator 
        /// </summary>
        [DataMember]
        [ProtoMember(21)]
        public Collection<Object> MessageParameters
        {
            get
            {
                return this._messageParameters;
            }
            set
            {
                this._messageParameters = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for rule map context
        /// </summary>
        [DataMember]
        [ProtoMember(22)]
        public Int32 RuleMapContextId
        {
            get
            {
                return this._ruleMapContextId;
            }
            set
            {
                this._ruleMapContextId = value;
            }
        }

        /// <summary>
        ///Indicates rule map context name
        /// </summary>
        [DataMember]
        [ProtoMember(23)]
        public String RuleMapContextName
        {
            get
            {
                return this._ruleMapContextName;
            }
            set
            {
                this._ruleMapContextName = value;
            }
        }

        /// <summary>
        /// Indicates id for rule
        /// </summary>
        [DataMember]
        [ProtoMember(24)]
        public Int32 RuleId
        {
            get
            {
                return this._ruleId;
            }
            set
            {
                this._ruleId = value;
            }
        }

        /// <summary>
        ///Indicates rule name
        /// </summary>
        [DataMember]
        [ProtoMember(25)]
        public String RuleName
        {
            get
            {
                return this._ruleName;
            }
            set
            {
                this._ruleName = value;
            }
        }


        /// <summary>
        /// Indicates operation result type 
        /// </summary>
        [DataMember]
        [ProtoMember(26)]
        public OperationResultType OperationResultType
        {
            get
            {
                return this._operationResultType;
            }
            set
            {
                this._operationResultType = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for Job ( Import or UI) 
        /// </summary>
        [DataMember]
        [ProtoMember(27)]
        public Int32 JobId
        {
            get
            {
                return this._jobId;
            }
            set
            {
                this._jobId = value;
            }
        }

        /// <summary>
        /// Indicates time stamp indicating when error was recorded.
        /// </summary>
        [DataMember]
        [ProtoMember(28)]
        public DateTime AuditTimeStamp
        {
            get
            {
                return this._auditTimeStamp;
            }
            set
            {
                this._auditTimeStamp = value;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <returns>String xml representation</returns>
        public override String ToXml()
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //EntityStateValidation node start
                    xmlWriter.WriteStartElement("EntityStateValidation");

                    #region write EntityStateValidation

                    xmlWriter.WriteAttributeString("SystemValidationStateAttribute", this.SystemValidationStateAttribute.ToString());
                    xmlWriter.WriteAttributeString("EntityId", this.EntityId.ToString());
                    xmlWriter.WriteAttributeString("EntityName", this.EntityName);
                    xmlWriter.WriteAttributeString("EntityLongName", this.EntityLongName);
                    xmlWriter.WriteAttributeString("EntityTypeLongName", this.EntityTypeLongName);
                    xmlWriter.WriteAttributeString("CategoryLongName", this.CategoryLongName);
                    xmlWriter.WriteAttributeString("ReasonType", this.ReasonType.ToString());
                    xmlWriter.WriteAttributeString("ContainerId", this.ContainerId.ToString());
                    xmlWriter.WriteAttributeString("ContainerName", this.ContainerName);
                    xmlWriter.WriteAttributeString("ContainerLongName", this.ContainerLongName);
                    xmlWriter.WriteAttributeString("RelationshipId", this.RelationshipId.ToString());
                    xmlWriter.WriteAttributeString("RelationshipTypeLongName", this.RelationshipTypeLongName);
                    xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                    xmlWriter.WriteAttributeString("RelatedEntityName", this.RelatedEntityName);
                    xmlWriter.WriteAttributeString("RelatedEntityLongName", this.RelatedEntityLongName);
                    xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                    xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                    xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
                    xmlWriter.WriteAttributeString("AttributeLongName", this.AttributeLongName);
                    xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                    xmlWriter.WriteAttributeString("MessageCode", this.MessageCode.ToString());
                    xmlWriter.WriteAttributeString("MessageParameters", (MessageParameters != null && MessageParameters.Count > 0) ? String.Join("#@#", MessageParameters) : String.Empty);
                    xmlWriter.WriteAttributeString("RuleMapContextId", this.RuleMapContextId.ToString());
                    xmlWriter.WriteAttributeString("RuleMapContextName", this.RuleMapContextName);
                    xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
                    xmlWriter.WriteAttributeString("RuleName", this.RuleName);
                    xmlWriter.WriteAttributeString("OperationResultType", this.OperationResultType.ToString());
                    xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                    xmlWriter.WriteAttributeString("AuditTimeStamp", this.AuditTimeStamp.ToString());

                    #endregion write EntityStateValidation

                    //EntityStateValidation node end
                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();

                    //get the actual XML
                    returnXml = sw.ToString();
                }
            }

            return returnXml;
        }

        /// <summary>
        /// Gives xml representation of an instance
        /// </summary>
        /// <param name="objectSerialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>String xml representation</returns>
        public override String ToXml(ObjectSerialization objectSerialization)
        {
            String returnXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    if (objectSerialization == ObjectSerialization.External)
                    {
                        //ValidationMessage node start
                        xmlWriter.WriteStartElement("ValidationMessage");

                        #region write EntityStateValidation

                        xmlWriter.WriteAttributeString("ReasonType", this.ReasonType.ToString());
                        xmlWriter.WriteAttributeString("RelationshipId", this.RelationshipId.ToString());
                        xmlWriter.WriteAttributeString("RelationshipTypeLongName", this.RelationshipTypeLongName);
                        xmlWriter.WriteAttributeString("RelatedEntityId", this.RelatedEntityId.ToString());
                        xmlWriter.WriteAttributeString("RelatedEntityName", this.RelatedEntityName);
                        xmlWriter.WriteAttributeString("RelatedEntityLongName", this.RelatedEntityLongName);
                        xmlWriter.WriteAttributeString("AttributeModelType", this.AttributeModelType.ToString());
                        xmlWriter.WriteAttributeString("AttributeId", this.AttributeId.ToString());
                        xmlWriter.WriteAttributeString("AttributeName", this.AttributeName);
                        xmlWriter.WriteAttributeString("AttributeLongName", this.AttributeLongName);
                        xmlWriter.WriteAttributeString("Locale", this.Locale.ToString());
                        xmlWriter.WriteAttributeString("MessageCode", this.MessageCode.ToString());
                        xmlWriter.WriteAttributeString("RuleMapContextId", this.RuleMapContextId.ToString());
                        xmlWriter.WriteAttributeString("RuleMapContextName", this.RuleMapContextName);
                        xmlWriter.WriteAttributeString("RuleId", this.RuleId.ToString());
                        xmlWriter.WriteAttributeString("RuleName", this.RuleName);
                        xmlWriter.WriteAttributeString("OperationResultType", this.OperationResultType.ToString());
                        xmlWriter.WriteAttributeString("Action", this.Action.ToString());
                        xmlWriter.WriteAttributeString("AuditTimeStamp", this.AuditTimeStamp.ToString());

                        #endregion write EntityStateValidation

                        //ValidationMessage node end
                        xmlWriter.WriteEndElement();
                    }

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
            if (obj is EntityStateValidation)
            {
                EntityStateValidation objectToBeCompared = obj as EntityStateValidation;

                if (this.EntityId != objectToBeCompared.EntityId)
                {
                    return false;
                }
                if (this.RelationshipId != objectToBeCompared.AttributeId)
                {
                    return false;
                }
                if (this.AttributeId != objectToBeCompared.AttributeId)
                {
                    return false;
                }
                if (this.ReasonType != objectToBeCompared.ReasonType)
                {
                    return false;
                }
                if (this.Locale != objectToBeCompared.Locale)
                {
                    return false;
                }
                if (this.MessageCode != objectToBeCompared.MessageCode)
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

            hashCode = this.EntityId.GetHashCode() ^ this.RelationshipId.GetHashCode() ^ this.AttributeId.GetHashCode() ^
                       this.ReasonType.GetHashCode() ^ this.Locale.GetHashCode() ^ this.MessageCode.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Compares EntityStateValidation object with current EntityStateValidation object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetEntityStateValidation">Indicates EntityStateValidation object to be compared with current EntityStateValidation object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(EntityStateValidation subsetEntityStateValidation, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.EntityId != subsetEntityStateValidation.EntityId)
                {
                    return false;
                }

                if (this.ContainerId != subsetEntityStateValidation.ContainerId)
                {
                    return false;
                }

                if (this.RelationshipId != subsetEntityStateValidation.RelationshipId)
                {
                    return false;
                }

                if (this.RelatedEntityId != subsetEntityStateValidation.RelatedEntityId)
                {
                    return false;
                }

                if (this.AttributeId != subsetEntityStateValidation.AttributeId)
                {
                    return false;
                }

                if (this.RuleId != subsetEntityStateValidation.RuleId)
                {
                    return false;
                }

                if (this.RuleMapContextId != subsetEntityStateValidation.RuleMapContextId)
                {
                    return false;
                }

                if (this.MessageCode != subsetEntityStateValidation.MessageCode)
                {
                    return false;
                }
            }

            if (!base.IsSuperSetOf(subsetEntityStateValidation, compareIds))
            {
                return false;
            }

            if (this.SystemValidationStateAttribute != subsetEntityStateValidation.SystemValidationStateAttribute)
            {
                return false;
            }

            if (String.Compare(this.EntityName, subsetEntityStateValidation.EntityName) != 0)
            {
                return false;
            }

            if (String.Compare(this.EntityLongName, subsetEntityStateValidation.EntityLongName) != 0)
            {
                return false;
            }

            if (String.Compare(this.CategoryLongName, subsetEntityStateValidation.CategoryLongName) != 0)
            {
                return false;
            }

            if (this.ReasonType != subsetEntityStateValidation.ReasonType)
            {
                return false;
            }

            if (String.Compare(this.ContainerName, subsetEntityStateValidation.ContainerName) != 0)
            {
                return false;
            }

            if (String.Compare(this.ContainerLongName, subsetEntityStateValidation.ContainerLongName) != 0)
            {
                return false;
            }

            if (this.AttributeModelType != subsetEntityStateValidation.AttributeModelType)
            {
                return false;
            }

            if (String.Compare(this.AttributeName, subsetEntityStateValidation.AttributeName) != 0)
            {
                return false;
            }

            if (String.Compare(this.AttributeLongName, subsetEntityStateValidation.AttributeLongName) != 0)
            {
                return false;
            }

            if (this.Locale != subsetEntityStateValidation.Locale)
            {
                return false;
            }

            if (this.OperationResultType != subsetEntityStateValidation.OperationResultType) 
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load entity validation state from xml 
        /// </summary>
        /// <param name="valuesAsXml"></param>
        private void LoadEntityValidationState(String valuesAsXml)
        {
            if (string.IsNullOrWhiteSpace(valuesAsXml))
            {
                return;
            }

            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityStateValidation")
                    {
                        #region Read EntityStateValidation

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("SystemValidationStateAttribute"))
                            {
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out this._systemValidationStateAttribute);
                                this.SystemValidationStateAttribute = _systemValidationStateAttribute;
                            }
                            if (reader.MoveToAttribute("EntityId"))
                            {
                                this.EntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._entityId);
                            }
                            if (reader.MoveToAttribute("EntityName"))
                            {
                                this.EntityName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("EntityLongName"))
                            {
                                this.EntityLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("EntityTypeLongName"))
                            {
                                this.EntityTypeLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("CategoryLongName"))
                            {
                                this.CategoryLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("ReasonType"))
                            {
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out this._reasonType);
                                this.ReasonType = this._reasonType;
                            }
                            if (reader.MoveToAttribute("ContainerId"))
                            {
                                this.ContainerId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._containerId);
                            }
                            if (reader.MoveToAttribute("ContainerName"))
                            {
                                this.ContainerName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("ContainerLongName"))
                            {
                                this.ContainerLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("RelationshipId"))
                            {
                                this.RelationshipId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relationshipId);
                            }
                            if (reader.MoveToAttribute("RelationshipTypeLongName"))
                            {
                                this.RelationshipTypeLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("RelatedEntityId"))
                            {
                                this.RelatedEntityId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), this._relatedEntityId);
                            }
                            if (reader.MoveToAttribute("RelatedEntityName"))
                            {
                                this.RelatedEntityName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("RelatedEntityLongName"))
                            {
                                this.RelatedEntityLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("AttributeModelType"))
                            {
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out this._attributeModelType);
                                this.AttributeModelType = this._attributeModelType;
                            }
                            if (reader.MoveToAttribute("AttributeId"))
                            {
                                this.AttributeId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._attributeId);
                            }
                            if (reader.MoveToAttribute("AttributeName"))
                            {
                                this.AttributeName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("AttributeLongName"))
                            {
                                this.AttributeLongName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("Locale"))
                            {
                                LocaleEnum _localEnum = LocaleEnum.UnKnown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out _localEnum);
                                this.Locale = _localEnum;
                            }
                            if (reader.MoveToAttribute("MessageCode"))
                            {
                                this.MessageCode = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("MessageParameters"))
                            {
                                this.MessageParameters = ValueTypeHelper.SplitStringToGenericCollection<Object>(reader.ReadContentAsString(), "#@#", StringSplitOptions.RemoveEmptyEntries);
                            }
                            if (reader.MoveToAttribute("RuleMapContextId"))
                            {
                                this.RuleMapContextId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleMapContextId);
                            }
                            if (reader.MoveToAttribute("RuleMapContextName"))
                            {
                                this.RuleMapContextName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("RuleId"))
                            {
                                this.RuleId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._ruleId);
                            }
                            if (reader.MoveToAttribute("RuleName"))
                            {
                                this.RuleName = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("OperationResultType"))
                            {
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out this._operationResultType);
                                this.OperationResultType = this._operationResultType;
                            }
                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction _action = ObjectAction.Unknown;
                                ValueTypeHelper.EnumTryParse(reader.ReadContentAsString(), true, out  _action);
                                this.Action = _action;
                            }
                            if (reader.MoveToAttribute("AuditTimeStamp"))
                            {
                                this.AuditTimeStamp = ValueTypeHelper.DateTimeTryParse(reader.ReadContentAsString(), DateTime.MinValue, this.Locale);
                            }
                        }
                        else
                        {
                            reader.Read();
                        }

                        #endregion Read EntityStateValidation
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