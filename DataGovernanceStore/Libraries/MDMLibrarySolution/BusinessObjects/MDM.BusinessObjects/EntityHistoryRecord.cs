using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity history record
    /// </summary>
    [DataContract]
    public class EntityHistoryRecord : MDMObject, IEntityHistoryRecord
    {
        #region Fields

        /// <summary>
        /// Field denoting the type of change happened
        /// </summary>
        private EntityChangeType _changeType = EntityChangeType.Unknown;

        /// <summary>
        /// Field to denoting long name of the entity which is modified 
        /// </summary>
        private String _changedData_EntityLongName = "NA";

        /// <summary>
        /// Field denoting category id which is modified 
        /// </summary>
        private Int64 _changedData_CategoryId = -1;

        /// <summary>
        /// Field denoting category long name which is modified 
        /// </summary>
        private String _changedData_CategoryLongName = "NA";

        /// <summary>
        /// Field denoting category long name which is modified 
        /// </summary>
        private String _changedData_CategoryLongNamePath = "NA";
        
        /// <summary>
        /// Field denoting Related entity id which is modified 
        /// </summary>
        private Int64 _changedData_RelatedEntityId = -1;

        /// <summary>
        /// Field denoting Related entity long name which is modified 
        /// </summary>
        private String  _changedData_RelatedEntityLongName = "NA";

        /// <summary>
        /// Field denoting Extension container id which is modified 
        /// </summary>
        private Int32 _changedData_ExtensionContainerId = -1;

        /// <summary>
        /// Field denoting Extension container long name which is modified 
        /// </summary>
        private String _changedData_ExtensionContainerLongName = "NA";

        /// <summary>
        /// Field denoting Extension category id to which entity was modified 
        /// </summary>
        private Int64 _changedData_ExtensionCategoryId = -1;

        /// <summary>
        /// Field denoting Extension category long name to which entity was modified 
        /// </summary>
        private String _changedData_ExtensionCategoryLongName = "NA";

        /// <summary>
        /// Field denoting Extension category long name to which entity was modified 
        /// </summary>
        private String _changedData_ExtensionCategoryLongNamePath = "NA";
        
        /// <summary>
        /// Field denoting Relationship type id which is modified 
        /// </summary>
        private Int32 _changedData_RelationshipTypeId = -1;

        /// <summary>
        /// Field denoting Relationship type long name which is modified 
        /// </summary>
        private String _changedData_RelationshipTypeLongName = "NA";

        /// <summary>
        /// Field denoting entity type id of related entity which is modified 
        /// </summary>
        private Int32 _changedData_RelatedEntityTypeId = -1;

        /// <summary>
        /// Field denoting entity type long name of related entity which is modified 
        /// </summary>
        private String _changedData_RelatedEntityTypeLongName = "NA";

        /// <summary>
        /// Field denoting entity long name for related entity's(which is modified) parent entity 
        /// </summary>
        private String _changedData_RelatedEntityParentLongName = "NA";

        /// <summary>
        /// Field denoting id of a related entity's parent entity
        /// </summary>
        private Int64 _changedData_RelatedEntityParentId = -1;

        /// <summary>
        /// Field denoting entity type long name of related entity's parent which is modified 
        /// </summary>
        private String _changedData_RelatedEntityParentEntityTypeLongName = "NA";

        /// <summary>
        /// Field denoting Attribute id which is modified 
        /// </summary>
        private Int32 _changedData_AttributeId = -1;

        /// <summary>
        /// Field denoting Attribute long name which is modified 
        /// </summary>
        private String _changedData_AttributeLongName = "NA";

        /// <summary>
        /// Field denoting Attribute parent long name which is modified 
        /// </summary>
        private String _changedData_AttributeParentLongName = "NA";
        
        /// <summary>
        /// Field denoting UOM id which is modified 
        /// </summary>
        private Int32 _changedData_UOMId = -1;

        /// <summary>
        /// Field denoting UOM which is modified 
        /// </summary>
        private String _changedData_UOM = "NA";
        
        /// <summary>
        /// Field denoting Relationship type id which is modified 
        /// </summary>
        private Int32 _changedData_Seq = -1;

        /// <summary>
        /// Property denoting lookup attribute WSID which got modified
        /// </summary>
        private Int32 _changedData_InstanceRefId = -1;

        /// <summary>
        /// Field denoting Attribute modified value 
        /// </summary>
        private String _changedData_AttrVal = "NA";

        /// <summary>
        /// Field denoting previous value of the current history record change
        /// </summary>
        private String _previousVal = "NA";

		/// <summary>
		/// Field denoting source who provided the data
		/// </summary>
		private String _source = "Unknown";

        /// <summary>
        /// Field denoting previous source who provided the data
        /// </summary>
        private String _previousSource = "Unknown";

        /// <summary>
        /// Field denoting name of the workflow in which entity has been modified
        /// </summary>
        private String _workflowName = "NA";

        /// <summary>
        /// Field denoting name of the workflow in which entity has been modified
        /// </summary>
        private String _workflowVersionName = "NA";

        /// <summary>
        /// Field denoting name of the workflow run time instance id in which entity has been modified
        /// </summary>
        private String _workflowRuntimeInstanceId = "NA";
        
        /// <summary>
        /// Field denoting name of the workflow activity long name in which entity has been modified
        /// </summary>
        private String _workflowActivityLongName = "NA";

        /// <summary>
        /// Field denoting name of the workflow action taken on entity
        /// </summary>
        private String _workflowActivityActionTaken = "NA";

        /// <summary>
        /// Field denoting name of the workflow comments for entity
        /// </summary>
        private String _workflowComments = "NA";

        /// <summary>
        /// Field denoting previously assigned user in the workflow for entity
        /// </summary>
        private String _workflowPreviousAssignedUser = "NA";

        /// <summary>
        /// Field denoting currently assigned user in the workflow for entity
        /// </summary>
        private String _workflowCurrentAssignedUser = "NA";

        /// <summary>
        /// Field denoting whether data is invalidated or not.
        /// </summary>
        private Boolean _isInvalidData = false;

        /// <summary>
        /// Field denoting details about what has been modified in an entity
        /// </summary>
        private String _details = "NA";

        /// <summary>
        /// denotes modified date and time - when changes have been done
        /// </summary>
        private DateTime? _modifiedDateTime = null;

        /// <summary>
        /// denotes modified user - who has done changes 
        /// </summary>
        private String _modifiedUser = "NA";

        /// <summary>
        /// denotes modified program - which program has done changes 
        /// </summary>
        private String _modifiedProgram = "NA";

        /// <summary>
        /// denotes modified day - like outlook day grouping
        /// </summary>
        private String _modifiedDay = "NA";

        /// <summary>
        /// Field denoting the promoted root entity identifier
        /// </summary>
        private Int64 _promotedRootEntityId = -1;

        /// <summary>
        /// Field denoting the promoted attribute names string
        /// </summary>
        private String _promotedAttributesString = "NA";

        /// <summary>
        /// Field denoting the promoted root entity long name
        /// </summary>
        private String _promotedRootEntityLongName = "NA";

        /// <summary>
        /// Field denoting the promote message code
        /// </summary>
        private String _promoteMessageCode = String.Empty;

        /// <summary>
        /// Field denoting the promote message parameters
        /// </summary>
        private String _promoteMessageParams = String.Empty;

        /// <summary>
        /// Field denoting the promote root entity type long name
        /// </summary>
        private string _promoteRootEntityTypeLongName = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityHistoryRecord()
            : base()
        { }

        /// <summary>
        /// Constructor which takes Xml as input
        /// </summary>
        /// <param name="valuesAsXml">XML having xml values</param>
        public EntityHistoryRecord(String valuesAsXml)
        {
            LoadEntityHistoryRecord(valuesAsXml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the type of change happened
        /// </summary>
        [DataMember]
        public EntityChangeType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }

        /// <summary>
        /// Property denoting long name of entity which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_EntityLongName
        {
            get { return _changedData_EntityLongName; }
            set { _changedData_EntityLongName = value; }
        }

        /// <summary>
        ///  Property denoting category id which is modified
        /// </summary>
        [DataMember]
        public Int64 ChangedData_CategoryId
        {
            get { return _changedData_CategoryId; }
            set { _changedData_CategoryId = value; }
        }

        /// <summary>
        ///  Property denoting category long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_CategoryLongName
        {
            get { return _changedData_CategoryLongName; }
            set { _changedData_CategoryLongName = value; }
        }

        /// <summary>
        ///  Property denoting category long name path which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_CategoryLongNamePath
        {
            get { return _changedData_CategoryLongNamePath; }
            set { _changedData_CategoryLongNamePath = value; }
        }

        /// <summary>
        ///  Property denoting Related Entity Id which is modified
        /// </summary>
        [DataMember]
        public Int64 ChangedData_RelatedEntityId
        {
            get { return _changedData_RelatedEntityId; }
            set { _changedData_RelatedEntityId = value; }
        }

        /// <summary>
        ///  Property denoting Related Entity long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_RelatedEntityLongName
        {
            get { return _changedData_RelatedEntityLongName; }
            set { _changedData_RelatedEntityLongName = value; }
        }

        /// <summary>
        ///  Property denoting extension container Id which is modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_ExtensionContainerId
        {
            get { return _changedData_ExtensionContainerId; }
            set { _changedData_ExtensionContainerId = value; }
        }

        /// <summary>
        ///  Property denoting extension container long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_ExtensionContainerLongName
        {
            get { return _changedData_ExtensionContainerLongName; }
            set { _changedData_ExtensionContainerLongName = value; }
        }

        /// <summary>
        ///  Property denoting extension category Id to which entity was modified
        /// </summary>
        [DataMember]
        public Int64 ChangedData_ExtensionCategoryId
        {
            get { return _changedData_ExtensionCategoryId; }
            set { _changedData_ExtensionCategoryId = value; }
        }

        /// <summary>
        ///  Property denoting extension category long name to which entity was modified
        /// </summary>
        [DataMember]
        public String ChangedData_ExtensionCategoryLongName
        {
            get { return _changedData_ExtensionCategoryLongName; }
            set { _changedData_ExtensionCategoryLongName = value; }
        }

        /// <summary>
        ///  Property denoting extension category long name path to which entity was modified
        /// </summary>
        [DataMember]
        public String ChangedData_ExtensionCategoryLongNamePath
        {
            get { return _changedData_ExtensionCategoryLongNamePath; }
            set { _changedData_ExtensionCategoryLongNamePath = value; }
        }

        /// <summary>
        /// Property denoting relationship type Id which is modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_RelationshipTypeId
        {
            get { return _changedData_RelationshipTypeId; }
            set { _changedData_RelationshipTypeId = value; }
        }

        /// <summary>
        /// Property denoting relationship type long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_RelationshipTypeLongName
        {
            get { return _changedData_RelationshipTypeLongName; }
            set { _changedData_RelationshipTypeLongName = value; }
        }

        /// <summary>
        /// Property denoting entity type Id of related entity  which is modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_RelatedEntityTypeId
        {
            get { return _changedData_RelatedEntityTypeId; }
            set { _changedData_RelatedEntityTypeId = value; }
        }

        /// <summary>
        /// Property denoting entity type long name of the related entity which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_RelatedEntityTypeLongName
        {
            get { return _changedData_RelatedEntityTypeLongName; }
            set { _changedData_RelatedEntityTypeLongName = value; }
        }

        /// <summary>
        /// Field denoting entity type long name of related entity's parent which is modified 
        /// </summary>
        [DataMember]
        public String ChangedData_RelatedEntityParentEntityTypeLongName
        {
            get { return _changedData_RelatedEntityParentEntityTypeLongName; }
            set { _changedData_RelatedEntityParentEntityTypeLongName = value; }
        }

        /// <summary>
        /// Field denoting entity long name for related entity's(which is modified) parent entity
        /// </summary>
        [DataMember]
        public String ChangedData_RelatedEntityParentLongName
        {
            get { return _changedData_RelatedEntityParentLongName; }
            set { _changedData_RelatedEntityParentLongName = value; }
        }

        /// <summary>
        /// Field denoting entity id for related entity's(which is modified) parent entity
        /// </summary>
        [DataMember]
        public Int64 ChangedData_RelatedEntityParentId
        {
            get { return _changedData_RelatedEntityParentId; }
            set { _changedData_RelatedEntityParentId = value; }
        }

        /// <summary>
        /// Property denoting attribute Id which is modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_AttributeId
        {
            get { return _changedData_AttributeId; }
            set { _changedData_AttributeId = value; }
        }

        /// <summary>
        /// Property denoting attribute long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_AttributeLongName
        {
            get { return _changedData_AttributeLongName; }
            set { _changedData_AttributeLongName = value; }
        }

        /// <summary>
        /// Property denoting attribute parent long name which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_AttributeParentLongName
        {
            get { return _changedData_AttributeParentLongName; }
            set { _changedData_AttributeParentLongName = value; }
        }

        /// <summary>
        /// Property denoting UOM id for an attribute which is modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_UOMId 
        {
            get { return _changedData_UOMId; }
            set { _changedData_UOMId = value; }
        }

        /// <summary>
        /// Property denoting UOM for an attribute which is modified
        /// </summary>
        [DataMember]
        public String ChangedData_UOM
        {
            get { return _changedData_UOM; }
            set { _changedData_UOM = value; }
        }

        /// <summary>
        /// Property denoting sequence of collection attribute values which are modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_Seq
        {
            get { return _changedData_Seq; }
            set { _changedData_Seq = value; }
        }

        /// <summary>
        /// Property denoting lookup attribute WSID which got modified
        /// </summary>
        [DataMember]
        public Int32 ChangedData_InstanceRefId
        {
            get { return _changedData_InstanceRefId; }
            set { _changedData_InstanceRefId = value; }
        }

        /// <summary>
        /// Property denoting attribute values which got modified
        /// </summary>
        [DataMember]
        public String ChangedData_AttrVal
        {
            get { return _changedData_AttrVal; }
            set { _changedData_AttrVal = value;}
        }
        
        /// <summary>
        /// Property denoting previous value of the current history record change
        /// </summary>
        [DataMember]
        public String PreviousVal
        {
            get { return _previousVal; }
            set { _previousVal = value;}
        }

		/// <summary>
		/// Property denoting source that provided the data
		/// </summary>
		[DataMember]
		public String Source
		{
			get { return _source; }
			set { _source = value; }
		}

        /// <summary>
        /// Property denoting previous source that provided the data
        /// </summary>
        [DataMember]
        public String PreviousSource
        {
            get { return _previousSource; }
            set { _previousSource = value; }
        }
        /// <summary>
        /// Property denoting name of the workflow in which entity has been modified
        /// </summary>
        [DataMember]
        public String WorkflowName 
        { 
            get { return _workflowName; }
            set { _workflowName = value; }    
        }

        /// <summary>
        /// Property denoting name of the workflow in which entity has been modified
        /// </summary>
        [DataMember]
        public String WorkflowVersionName
        {
            get { return _workflowVersionName; }
            set { _workflowVersionName = value; }
        }

        /// <summary>
        /// Property denoting name of the workflow run time instance id in which entity has been modified
        /// </summary>
        [DataMember]
        public String WorkflowRuntimeInstanceId
        {
            get { return _workflowRuntimeInstanceId; }
            set { _workflowRuntimeInstanceId = value; }
        }

        /// <summary>
        /// Property denoting name of the workflow activity long name in which entity has been modified
        /// </summary>
        [DataMember]
        public String WorkflowActivityLongName
        {
            get { return _workflowActivityLongName; }
            set { _workflowActivityLongName = value; }
        }

        /// <summary>
        /// Property denoting name of the workflow action taken on entity
        /// </summary>
        [DataMember]
        public String WorkflowActivityActionTaken
        {
            get { return _workflowActivityActionTaken; }
            set { _workflowActivityActionTaken = value; }
        }

        /// <summary>
        /// Property denoting name of the workflow comments for entity
        /// </summary>
        [DataMember]
        public String WorkflowComments
        {
            get { return _workflowComments; }
            set { _workflowComments = value; }
        }

        /// <summary>
        /// Property denoting previously assigned user in the workflow for entity
        /// </summary>
        [DataMember]
        public String WorkflowPreviousAssignedUser
        {
            get { return _workflowPreviousAssignedUser; }
            set { _workflowPreviousAssignedUser = value; }
        }

        /// <summary>
        /// Property denoting currently assigned user in the workflow for entity
        /// </summary>
        [DataMember]
        public String WorkflowCurrentAssignedUser
        {
            get { return _workflowCurrentAssignedUser; }
            set { _workflowCurrentAssignedUser = value; }
        }

        /// <summary>
        /// Property denoting whether data is invalidated or not
        /// </summary>
        [DataMember]
        public Boolean IsInvalidData
        {
            get { return _isInvalidData; }
            set { _isInvalidData = value; }
        }

        /// <summary>
        /// Property denoting details about what has been modified in an entity
        /// </summary>
        [DataMember]
        public String Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// Property denoting modified date and time - when changes have been done
        /// </summary>
        [DataMember]
        public DateTime? ModifiedDateTime
        {
            get { return _modifiedDateTime; }
            set { _modifiedDateTime = value; }
        }

        /// <summary>
        /// Property denoting modified user - who has done changes 
        /// </summary>
        [DataMember]
        public String ModifiedUser
        {
            get { return _modifiedUser; }
            set { _modifiedUser = value; }
        }

        /// <summary>
        /// Property denoting modified program - which program has done changes 
        /// </summary>
        [DataMember]
        public String ModifiedProgram
        {
            get { return _modifiedProgram; }
            set { _modifiedProgram = value; }
        }

        /// <summary>
        /// Property denoting modified day - like outlook day grouping
        /// </summary>
        [DataMember]
        public String ModifiedDay
        {
            get { return _modifiedDay; }
            set { _modifiedDay = value; }
        }

        /// <summary>
        /// Property denoting the promoted root entity identifier.
        /// </summary>
        [DataMember]
        public Int64 PromotedRootEntityId
        {
            get { return _promotedRootEntityId; }
            set { _promotedRootEntityId = value; }
        }

        /// <summary>
        /// Property denoting the promoted attribute names string.
        /// </summary>
        [DataMember]
        public string PromotedAttributesString
        {
            get { return _promotedAttributesString; }
            set { _promotedAttributesString = value; }
        }

        /// <summary>
        /// Property denoting promoted root entity long name
        /// </summary>
        [DataMember]
        public String PromotedRootEntityLongName
        {
            get { return _promotedRootEntityLongName; }
            set { _promotedRootEntityLongName = value; }
        }

        /// <summary>
        /// Property denoting the promote message code.
        /// </summary>
        [DataMember]
        public String PromoteMessageCode
        {
            get { return _promoteMessageCode; }
            set { _promoteMessageCode = value; }
        }

        /// <summary>
        /// Property denoting the promote message params.
        /// </summary>
        [DataMember]
        public String PromoteMessageParams
        {
            get { return _promoteMessageParams; }
            set { _promoteMessageParams = value; }
        }

        /// <summary>
        /// Property denoting the long name of the promote root entity type.
        /// </summary>
        [DataMember]
        public String PromoteRootEntityTypeLongName
        {
            get { return _promoteRootEntityTypeLongName; }
            set { _promoteRootEntityTypeLongName = value; }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents EntityHistoryRecord  in Xml format
        /// </summary>
        /// <returns>String representation of current EntityHistoryRecord object</returns>
        public override String ToXml()
        {
            String xml = String.Empty;
            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);

            String modifiedDate = String.Empty;

            if (this.ModifiedDateTime != null)
            {
                modifiedDate = this.ModifiedDateTime.ToString();
            }

            // EntityHistoryRecord node start
            xmlWriter.WriteStartElement("EntityHistoryRecord");

            xmlWriter.WriteAttributeString("ChangeType", this.ChangeType.ToString());
            xmlWriter.WriteAttributeString("EntityLongName", this.ChangedData_EntityLongName);
            xmlWriter.WriteAttributeString("CategoryId", this.ChangedData_CategoryId.ToString());
            xmlWriter.WriteAttributeString("CategoryLongName", this.ChangedData_CategoryLongName);
            xmlWriter.WriteAttributeString("CategoryLongNamePath", this.ChangedData_CategoryLongNamePath);
            xmlWriter.WriteAttributeString("RelatedEntityId", this.ChangedData_RelatedEntityId.ToString());
            xmlWriter.WriteAttributeString("RelatedEntityLongName", this.ChangedData_RelatedEntityLongName);
            xmlWriter.WriteAttributeString("ExtensionContainerId", this.ChangedData_ExtensionContainerId.ToString());
            xmlWriter.WriteAttributeString("ExtensionContainerLongName", this.ChangedData_ExtensionContainerLongName);
            xmlWriter.WriteAttributeString("ExtensionCategoryId", this.ChangedData_ExtensionCategoryId.ToString());
            xmlWriter.WriteAttributeString("ExtensionCategoryLongName", this._changedData_ExtensionCategoryLongName);
            xmlWriter.WriteAttributeString("ExtensionCategoryLongNamePath", this._changedData_ExtensionCategoryLongNamePath);
            xmlWriter.WriteAttributeString("RelationshipTypeId", this.ChangedData_RelationshipTypeId.ToString());
            xmlWriter.WriteAttributeString("RelationshipTypeLongName", this.ChangedData_RelationshipTypeLongName);
            xmlWriter.WriteAttributeString("RelatedEntityTypeId", this.ChangedData_RelatedEntityTypeId.ToString());
            xmlWriter.WriteAttributeString("RelatedEntityTypeLongName", this.ChangedData_RelatedEntityTypeLongName);
            xmlWriter.WriteAttributeString("RelatedEntityParentEntityTypeLongName", this.ChangedData_RelatedEntityParentEntityTypeLongName);
            xmlWriter.WriteAttributeString("RelatedEntityParentLongName", this.ChangedData_RelatedEntityParentLongName);
            xmlWriter.WriteAttributeString("RelatedEntityParentId", this.ChangedData_RelatedEntityParentId.ToString());
            xmlWriter.WriteAttributeString("AttributeId", this.ChangedData_AttributeId.ToString());
            xmlWriter.WriteAttributeString("AttributeLongName", this.ChangedData_AttributeLongName);
            xmlWriter.WriteAttributeString("AttributeParentLongName", this.ChangedData_AttributeParentLongName);
            xmlWriter.WriteAttributeString("AttributeValue", this.ChangedData_AttrVal);
            xmlWriter.WriteAttributeString("InstanceRefId", this.ChangedData_InstanceRefId.ToString());
            xmlWriter.WriteAttributeString("PreviousValue", this.PreviousVal);
            xmlWriter.WriteAttributeString("UOMId", this.ChangedData_UOMId.ToString());
            xmlWriter.WriteAttributeString("UOM", this.ChangedData_UOM);
            xmlWriter.WriteAttributeString("Sequence", this.ChangedData_Seq.ToString());
			xmlWriter.WriteAttributeString("Source", this.Source);
            xmlWriter.WriteAttributeString("PreviousSource", this.PreviousSource);
            xmlWriter.WriteAttributeString("WorkflowName", this.WorkflowName);
            xmlWriter.WriteAttributeString("WorkflowVersionName", this.WorkflowVersionName);
            xmlWriter.WriteAttributeString("WorkflowRuntimeInstanceId", this.WorkflowRuntimeInstanceId);
            xmlWriter.WriteAttributeString("WorkflowActivityLongName", this.WorkflowActivityLongName);
            xmlWriter.WriteAttributeString("WorkflowActivityActionTaken", this.WorkflowActivityActionTaken);
            xmlWriter.WriteAttributeString("WorkflowComments", this.WorkflowComments);
            xmlWriter.WriteAttributeString("WorkflowPreviousAssignedUser", this.WorkflowPreviousAssignedUser.ToString());
            xmlWriter.WriteAttributeString("WorkflowCurrentAssignedUser", this.WorkflowCurrentAssignedUser.ToString());
            xmlWriter.WriteAttributeString("IsInvalidData", this.IsInvalidData.ToString());
            xmlWriter.WriteAttributeString("Details", this.Details);
            xmlWriter.WriteAttributeString("ModifiedDate", modifiedDate);
            xmlWriter.WriteAttributeString("ModifiedUser", this.ModifiedUser);
            xmlWriter.WriteAttributeString("ModifiedProgram", this.ModifiedProgram);
            xmlWriter.WriteAttributeString("AuditRefId", this.AuditRefId.ToString());
            xmlWriter.WriteAttributeString("Action", this.Action.ToString());
            xmlWriter.WriteAttributeString("ModifiedDay", this.ModifiedDay);
            xmlWriter.WriteAttributeString("PromotedRootEntityId", this.PromotedRootEntityId.ToString());
            xmlWriter.WriteAttributeString("PromotedRootEntityLongName", this.PromotedRootEntityLongName);
            xmlWriter.WriteAttributeString("PromotedAttributesString", this.PromotedAttributesString);
            xmlWriter.WriteAttributeString("PromoteMessageCode", this.PromoteMessageCode);
            xmlWriter.WriteAttributeString("PromoteMessageParams", this.PromoteMessageParams);
            xmlWriter.WriteAttributeString("PromoteRootEntityTypeLongName", this.PromoteRootEntityTypeLongName);

            // EntityHistoryRecord end node
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            // get the actual XML
            xml = sw.ToString();

            xmlWriter.Close();
            sw.Close();

            return xml;
        }

        /// <summary>
        /// clones the object to another object
        /// </summary>
        /// <returns></returns>
        public EntityHistoryRecord Clone()
        {
            EntityHistoryRecord clonedEntityHistoryRecord = new EntityHistoryRecord();

            clonedEntityHistoryRecord.ChangeType = this.ChangeType;
            clonedEntityHistoryRecord.ChangedData_EntityLongName = this.ChangedData_EntityLongName;
            clonedEntityHistoryRecord.ChangedData_CategoryId = this.ChangedData_CategoryId;
            clonedEntityHistoryRecord.ChangedData_CategoryLongName = this.ChangedData_CategoryLongName;
            clonedEntityHistoryRecord.ChangedData_CategoryLongNamePath = this.ChangedData_CategoryLongNamePath;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityId = this.ChangedData_RelatedEntityId;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityLongName = this.ChangedData_RelatedEntityLongName;
            clonedEntityHistoryRecord.ChangedData_ExtensionContainerId = this.ChangedData_ExtensionContainerId;
            clonedEntityHistoryRecord.ChangedData_ExtensionContainerLongName = this.ChangedData_ExtensionContainerLongName;
            clonedEntityHistoryRecord.ChangedData_ExtensionCategoryId = this.ChangedData_ExtensionCategoryId;
            clonedEntityHistoryRecord.ChangedData_ExtensionCategoryLongName = this.ChangedData_ExtensionCategoryLongName;
            clonedEntityHistoryRecord.ChangedData_ExtensionCategoryLongNamePath = this.ChangedData_ExtensionCategoryLongNamePath;
            clonedEntityHistoryRecord.ChangedData_RelationshipTypeId = this.ChangedData_RelationshipTypeId;
            clonedEntityHistoryRecord.ChangedData_RelationshipTypeLongName = this.ChangedData_RelationshipTypeLongName;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityTypeId = this.ChangedData_RelatedEntityTypeId;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityTypeLongName = this.ChangedData_RelatedEntityTypeLongName;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityParentEntityTypeLongName = this.ChangedData_RelatedEntityParentEntityTypeLongName;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityParentLongName = this.ChangedData_RelatedEntityParentLongName;
            clonedEntityHistoryRecord.ChangedData_RelatedEntityParentId = this.ChangedData_RelatedEntityParentId;
            clonedEntityHistoryRecord.ChangedData_AttributeId = this.ChangedData_AttributeId;
            clonedEntityHistoryRecord.ChangedData_AttributeLongName = this.ChangedData_AttributeLongName;
            clonedEntityHistoryRecord.ChangedData_AttributeParentLongName = this.ChangedData_AttributeParentLongName;
            clonedEntityHistoryRecord.ChangedData_UOMId = this.ChangedData_UOMId;
            clonedEntityHistoryRecord.ChangedData_UOM = this.ChangedData_UOM;
            clonedEntityHistoryRecord.ChangedData_InstanceRefId = this.ChangedData_InstanceRefId;
            clonedEntityHistoryRecord.ChangedData_AttrVal = this.ChangedData_AttrVal;
            clonedEntityHistoryRecord.ChangedData_Seq = this.ChangedData_Seq;
            clonedEntityHistoryRecord.PreviousVal = this.PreviousVal;
			clonedEntityHistoryRecord.Source = this.Source;
            clonedEntityHistoryRecord.PreviousSource = this.PreviousSource;
            clonedEntityHistoryRecord.WorkflowName = this.WorkflowName;
            clonedEntityHistoryRecord.WorkflowVersionName = this.WorkflowVersionName;
            clonedEntityHistoryRecord.WorkflowRuntimeInstanceId = this.WorkflowRuntimeInstanceId;
            clonedEntityHistoryRecord.WorkflowActivityLongName = this.WorkflowActivityLongName;
            clonedEntityHistoryRecord.WorkflowActivityActionTaken = this.WorkflowActivityActionTaken;
            clonedEntityHistoryRecord.WorkflowComments = this.WorkflowComments;
            clonedEntityHistoryRecord.WorkflowPreviousAssignedUser = this.WorkflowPreviousAssignedUser;
            clonedEntityHistoryRecord.WorkflowCurrentAssignedUser = this.WorkflowCurrentAssignedUser;
            clonedEntityHistoryRecord.IsInvalidData = this.IsInvalidData;
            clonedEntityHistoryRecord.Details = this.Details;
            clonedEntityHistoryRecord.ModifiedDateTime = this.ModifiedDateTime;
            clonedEntityHistoryRecord.ModifiedUser = this.ModifiedUser;
            clonedEntityHistoryRecord.ModifiedProgram = this.ModifiedProgram;
            clonedEntityHistoryRecord.AuditRefId = this.AuditRefId;
            clonedEntityHistoryRecord.Action = this.Action;
            clonedEntityHistoryRecord.ModifiedDay = this.ModifiedDay;
            clonedEntityHistoryRecord.PromotedAttributesString = this.PromotedAttributesString;
            clonedEntityHistoryRecord.PromotedRootEntityId = this.PromotedRootEntityId;
            clonedEntityHistoryRecord.PromotedRootEntityLongName = this.PromotedRootEntityLongName;
            clonedEntityHistoryRecord.PromoteMessageCode = this.PromoteMessageCode;
            clonedEntityHistoryRecord.PromoteMessageParams = this.PromoteMessageParams;
            clonedEntityHistoryRecord.PromoteRootEntityTypeLongName = this.PromoteRootEntityTypeLongName;

            return clonedEntityHistoryRecord;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="objectToBeCompared">EntityHistoryRecord Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean Equals(EntityHistoryRecord objectToBeCompared, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.ChangedData_CategoryId != objectToBeCompared.ChangedData_CategoryId)
                {
                    return false;
                }

                if (this.ChangedData_RelatedEntityId != objectToBeCompared.ChangedData_RelatedEntityId)
                {
                    return false;
                }

                if (this.ChangedData_ExtensionContainerId != objectToBeCompared.ChangedData_ExtensionContainerId)
                {
                    return false;
                }

                if (this.ChangedData_ExtensionCategoryId != objectToBeCompared.ChangedData_ExtensionCategoryId)
                {
                    return false;
                }

                if (this.ChangedData_RelationshipTypeId != objectToBeCompared.ChangedData_RelationshipTypeId)
                {
                    return false;
                }

                if (this.ChangedData_RelatedEntityTypeId != objectToBeCompared.ChangedData_RelatedEntityTypeId)
                {
                    return false;
                }

                if (this.ChangedData_RelatedEntityParentId != objectToBeCompared.ChangedData_RelatedEntityParentId)
                {
                    return false;
                }

                if (this.ChangedData_AttributeId != objectToBeCompared.ChangedData_AttributeId)
                {
                    return false;
                }

                if (this.ChangedData_UOMId != objectToBeCompared.ChangedData_UOMId)
                {
                    return false;
                }

                if (this.ChangedData_InstanceRefId != objectToBeCompared.ChangedData_InstanceRefId)
                {
                    return false;
                }

                if (this.WorkflowRuntimeInstanceId != objectToBeCompared.WorkflowRuntimeInstanceId)
                {
                    return false;
                }

                if (this.AuditRefId != objectToBeCompared.AuditRefId)
                {
                    return false;
                }

                if (this.PromotedRootEntityId != objectToBeCompared.PromotedRootEntityId)
                {
                    return false;
                }
            }

            if (this.ChangedData_EntityLongName != objectToBeCompared.ChangedData_EntityLongName)
            {
                return false;
            }

            if (this.ChangeType != objectToBeCompared.ChangeType)
            {
                return false;
            }

            if (this.ChangedData_CategoryLongName != objectToBeCompared.ChangedData_CategoryLongName)
            {
                return false;
            }

            if (this.ChangedData_CategoryLongNamePath != objectToBeCompared.ChangedData_CategoryLongNamePath)
            {
                return false;
            }

            if (this.ChangedData_RelatedEntityLongName != objectToBeCompared.ChangedData_RelatedEntityLongName)
            {
                return false;
            }

            if (this.ChangedData_ExtensionContainerLongName != objectToBeCompared.ChangedData_ExtensionContainerLongName)
            {
                return false;
            }

            if (this.ChangedData_ExtensionCategoryLongName != objectToBeCompared.ChangedData_ExtensionCategoryLongName)
            {
                return false;
            }

            if (this.ChangedData_ExtensionCategoryLongNamePath != objectToBeCompared.ChangedData_ExtensionCategoryLongNamePath)
            {
                return false;
            }

            if (this.ChangedData_RelationshipTypeLongName != objectToBeCompared.ChangedData_RelationshipTypeLongName)
            {
                return false;
            }

            if (this.ChangedData_RelatedEntityTypeLongName != objectToBeCompared.ChangedData_RelatedEntityTypeLongName)
            {
                return false;
            }

            if (this.ChangedData_RelatedEntityParentEntityTypeLongName != objectToBeCompared.ChangedData_RelatedEntityParentEntityTypeLongName)
            {
                return false;
            }

            if (this.ChangedData_RelatedEntityParentLongName != objectToBeCompared.ChangedData_RelatedEntityParentLongName)
            {
                return false;
            }

            if (this.ChangedData_AttributeLongName != objectToBeCompared.ChangedData_AttributeLongName)
            {
                return false;
            }

            if (this.ChangedData_AttributeParentLongName != objectToBeCompared.ChangedData_AttributeParentLongName)
            {
                return false;
            }

            if (this.ChangedData_UOM != objectToBeCompared.ChangedData_UOM)
            {
                return false;
            }

            if (this.ChangedData_AttrVal != objectToBeCompared.ChangedData_AttrVal)
            {
                return false;
            }

            if (this.PreviousVal != objectToBeCompared.PreviousVal)
            {
                return false;
            }

            if (this.ChangedData_Seq != objectToBeCompared.ChangedData_Seq)
            {
                return false;
            }

            if (this.Source != objectToBeCompared.Source)
            {
                return false;
            }

            if (this.PreviousSource != objectToBeCompared.PreviousSource)
            {
                return false;
            }

            if (this.WorkflowName != objectToBeCompared.WorkflowName)
            {
                return false;
            }

            if (this.WorkflowVersionName != objectToBeCompared.WorkflowVersionName)
            {
                return false;
            }

            if (this.WorkflowActivityLongName != objectToBeCompared.WorkflowActivityLongName)
            {
                return false;
            }

            if (this.WorkflowActivityActionTaken != objectToBeCompared.WorkflowActivityActionTaken)
            {
                return false;
            }

            if (this.WorkflowComments != objectToBeCompared.WorkflowComments)
            {
                return false;
            }

            if (this.WorkflowPreviousAssignedUser != objectToBeCompared.WorkflowPreviousAssignedUser)
            {
                return false;
            }

            if (this.WorkflowCurrentAssignedUser != objectToBeCompared.WorkflowCurrentAssignedUser)
            {
                return false;
            }

            if (this.IsInvalidData != objectToBeCompared.IsInvalidData)
            {
                return false;
            }

            if (this.Details != objectToBeCompared.Details)
            {
                return false;
            }

            if (this.ModifiedUser != objectToBeCompared.ModifiedUser)
            {
                return false;
            }

            if (this.ModifiedProgram != objectToBeCompared.ModifiedProgram)
            {
                return false;
            }

            if (this.Action != objectToBeCompared.Action)
            {
                return false;
            }

            if (this.ModifiedDay != objectToBeCompared.ModifiedDay)
            {
                return false;
            }

            if (this.PromotedAttributesString != objectToBeCompared.PromotedAttributesString)
            {
                return false;
            }

            if (this.PromotedRootEntityLongName != objectToBeCompared.PromotedRootEntityLongName)
            {
                return false;
            }
            
            if (this.PromoteMessageCode != objectToBeCompared.PromoteMessageCode)
            {
                return false;
            }
            
            if (this.PromoteMessageParams != objectToBeCompared.PromoteMessageParams)
            {
                return false;
            }

            if (this.PromoteRootEntityTypeLongName != objectToBeCompared.PromoteRootEntityTypeLongName)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="subsetEntityHistoryRecord">EntityHistoryRecord Object to compare with the current Object.</param>
        /// <param name="compareIds">Flag to say whether Id based properties has to be considered in comparison or not.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public Boolean IsSuperSetOf(EntityHistoryRecord subsetEntityHistoryRecord, Boolean compareIds = false)
        {
            if (subsetEntityHistoryRecord != null)
            {
                if (base.IsSuperSetOf(subsetEntityHistoryRecord, compareIds))
                {
                    if (compareIds)
                    {
                        if (this.ChangedData_CategoryId != subsetEntityHistoryRecord.ChangedData_CategoryId)
                        {
                            return false;
                        }

                        if (this.ChangedData_RelatedEntityId != subsetEntityHistoryRecord.ChangedData_RelatedEntityId)
                        {
                            return false;
                        }

                        if (this.ChangedData_ExtensionContainerId != subsetEntityHistoryRecord.ChangedData_ExtensionContainerId)
                        {
                            return false;
                        }

                        if (this.ChangedData_ExtensionCategoryId != subsetEntityHistoryRecord.ChangedData_ExtensionCategoryId)
                        {
                            return false;
                        }

                        if (this.ChangedData_RelationshipTypeId != subsetEntityHistoryRecord.ChangedData_RelationshipTypeId)
                        {
                            return false;
                        }

                        if (this.ChangedData_RelatedEntityTypeId != subsetEntityHistoryRecord.ChangedData_RelatedEntityTypeId)
                        {
                            return false;
                        }

                        if (this.ChangedData_RelatedEntityParentId != subsetEntityHistoryRecord.ChangedData_RelatedEntityParentId)
                        {
                            return false;
                        }

                        if (this.ChangedData_AttributeId != subsetEntityHistoryRecord.ChangedData_AttributeId)
                        {
                            return false;
                        }

                        if (this.ChangedData_UOMId != subsetEntityHistoryRecord.ChangedData_UOMId)
                        {
                            return false;
                        }

                        if (this.ChangedData_InstanceRefId != subsetEntityHistoryRecord.ChangedData_InstanceRefId)
                        {
                            return false;
                        }

                        if (this.WorkflowRuntimeInstanceId != subsetEntityHistoryRecord.WorkflowRuntimeInstanceId)
                        {
                            return false;
                        }

                        if (this.AuditRefId != subsetEntityHistoryRecord.AuditRefId)
                        {
                            return false;
                        }
                        
                        if (this.PromotedRootEntityId != subsetEntityHistoryRecord.PromotedRootEntityId)
                        {
                            return false;
                        }
                    }

                    if (this.ChangedData_EntityLongName != subsetEntityHistoryRecord.ChangedData_EntityLongName)
                    {
                        return false;
                    }

                    if (this.ChangeType != subsetEntityHistoryRecord.ChangeType)
                    {
                        return false;
                    }

                    if (this.ChangedData_CategoryLongName != subsetEntityHistoryRecord.ChangedData_CategoryLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_CategoryLongNamePath != subsetEntityHistoryRecord.ChangedData_CategoryLongNamePath)
                    {
                        return false;
                    }

                    if (this.ChangedData_RelatedEntityLongName != subsetEntityHistoryRecord.ChangedData_RelatedEntityLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_ExtensionContainerLongName != subsetEntityHistoryRecord.ChangedData_ExtensionContainerLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_ExtensionCategoryLongName != subsetEntityHistoryRecord.ChangedData_ExtensionCategoryLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_ExtensionCategoryLongNamePath != subsetEntityHistoryRecord.ChangedData_ExtensionCategoryLongNamePath)
                    {
                        return false;
                    }

                    if (this.ChangedData_RelationshipTypeLongName != subsetEntityHistoryRecord.ChangedData_RelationshipTypeLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_RelatedEntityTypeLongName != subsetEntityHistoryRecord.ChangedData_RelatedEntityTypeLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_RelatedEntityParentEntityTypeLongName != subsetEntityHistoryRecord.ChangedData_RelatedEntityParentEntityTypeLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_RelatedEntityParentLongName != subsetEntityHistoryRecord.ChangedData_RelatedEntityParentLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_AttributeLongName != subsetEntityHistoryRecord.ChangedData_AttributeLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_AttributeParentLongName != subsetEntityHistoryRecord.ChangedData_AttributeParentLongName)
                    {
                        return false;
                    }

                    if (this.ChangedData_UOM != subsetEntityHistoryRecord.ChangedData_UOM)
                    {
                        return false;
                    }

                    if (this.ChangedData_AttrVal != subsetEntityHistoryRecord.ChangedData_AttrVal)
                    {
                        return false;
                    }

                    if (this.PreviousVal != subsetEntityHistoryRecord.PreviousVal)
                    {
                        return false;
                    }

                    if (this.ChangedData_Seq != subsetEntityHistoryRecord.ChangedData_Seq)
                    {
                        return false;
                    }

                    if (this.Source != subsetEntityHistoryRecord.Source)
                    {
                        return false;
                    }

                    if (this.PreviousSource != subsetEntityHistoryRecord.PreviousSource)
                    {
                        return false;
                    }

                    if (this.WorkflowName != subsetEntityHistoryRecord.WorkflowName)
                    {
                        return false;
                    }

                    if (this.WorkflowVersionName != subsetEntityHistoryRecord.WorkflowVersionName)
                    {
                        return false;
                    }

                    if (this.WorkflowActivityLongName != subsetEntityHistoryRecord.WorkflowActivityLongName)
                    {
                        return false;
                    }

                    if (this.WorkflowActivityActionTaken != subsetEntityHistoryRecord.WorkflowActivityActionTaken)
                    {
                        return false;
                    }

                    if (this.WorkflowComments != subsetEntityHistoryRecord.WorkflowComments)
                    {
                        return false;
                    }

                    if (this.WorkflowPreviousAssignedUser != subsetEntityHistoryRecord.WorkflowPreviousAssignedUser)
                    {
                        return false;
                    }

                    if (this.WorkflowCurrentAssignedUser != subsetEntityHistoryRecord.WorkflowCurrentAssignedUser)
                    {
                        return false;
                    }

                    if (this.IsInvalidData != subsetEntityHistoryRecord.IsInvalidData)
                    {
                        return false;
                    }

                    if (this.Details != subsetEntityHistoryRecord.Details)
                    {
                        return false;
                    }

                    if (this.ModifiedUser != subsetEntityHistoryRecord.ModifiedUser)
                    {
                        return false;
                    }

                    if (this.ModifiedProgram != subsetEntityHistoryRecord.ModifiedProgram)
                    {
                        return false;
                    }

                    if (this.Action != subsetEntityHistoryRecord.Action)
                    {
                        return false;
                    }

                    if (this.ModifiedDay != subsetEntityHistoryRecord.ModifiedDay)
                    {
                        return false;
                    }

                    if (this.PromotedAttributesString != subsetEntityHistoryRecord.PromotedAttributesString)
                    {
                        return false;
                    }

                    if (this.PromotedRootEntityLongName != subsetEntityHistoryRecord.PromotedRootEntityLongName)
                    {
                        return false;
                    }

                    if (this.PromoteMessageCode != subsetEntityHistoryRecord.PromoteMessageCode)
                    {
                        return false;
                    }

                    if (this.PromoteMessageParams != subsetEntityHistoryRecord.PromoteMessageParams)
                    {
                        return false;
                    }

                    if (this.PromoteRootEntityTypeLongName != subsetEntityHistoryRecord.PromoteRootEntityTypeLongName)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode() ^ this._changeType.GetHashCode() ^
                        this._changedData_EntityLongName.GetHashCode() ^
                        this._changedData_CategoryId.GetHashCode() ^
                        this._changedData_CategoryLongName.GetHashCode() ^
                        this._changedData_CategoryLongNamePath.GetHashCode() ^
                        this._changedData_RelatedEntityId.GetHashCode() ^
                        this._changedData_RelatedEntityLongName.GetHashCode() ^
                        this._changedData_ExtensionContainerId.GetHashCode() ^
                        this._changedData_ExtensionContainerLongName.GetHashCode() ^
                        this._changedData_ExtensionCategoryId.GetHashCode() ^
                        this._changedData_ExtensionCategoryLongName.GetHashCode() ^
                        this._changedData_ExtensionCategoryLongNamePath.GetHashCode() ^
                        this._changedData_RelationshipTypeId.GetHashCode() ^
                        this._changedData_RelationshipTypeLongName.GetHashCode() ^
                        this._changedData_RelatedEntityTypeId.GetHashCode() ^
                        this._changedData_RelatedEntityTypeLongName.GetHashCode() ^
                        this._changedData_RelatedEntityParentEntityTypeLongName.GetHashCode() ^
                        this._changedData_RelatedEntityParentLongName.GetHashCode() ^
                        this._changedData_RelatedEntityParentId.GetHashCode() ^
                        this._changedData_AttributeId.GetHashCode() ^
                        this._changedData_AttributeLongName.GetHashCode() ^
                        this._changedData_AttributeParentLongName.GetHashCode() ^
                        this._changedData_UOMId.GetHashCode() ^
                        this._changedData_UOM.GetHashCode() ^
                        this._changedData_Seq.GetHashCode() ^
                        this._changedData_InstanceRefId.GetHashCode() ^
                        this._changedData_AttrVal.GetHashCode() ^
                        this._previousVal.GetHashCode() ^
                        this._source.GetHashCode() ^
                        this._previousSource.GetHashCode() ^
                        this._workflowName.GetHashCode() ^
                        this._workflowVersionName.GetHashCode() ^
                        this._workflowRuntimeInstanceId.GetHashCode() ^
                        this._workflowActivityLongName.GetHashCode() ^
                        this._workflowActivityActionTaken.GetHashCode() ^
                        this._workflowComments.GetHashCode() ^
                        this._workflowPreviousAssignedUser.GetHashCode() ^
                        this._workflowCurrentAssignedUser.GetHashCode() ^
                        this._isInvalidData.GetHashCode() ^
                        this._details.GetHashCode() ^
                        this._modifiedDateTime.GetHashCode() ^
                        this._modifiedUser.GetHashCode() ^
                        this._modifiedProgram.GetHashCode() ^
                        this._modifiedDay.GetHashCode() ^
                        this._promotedAttributesString.GetHashCode() ^
                        this._promotedRootEntityId.GetHashCode() ^
                        this._promotedRootEntityLongName.GetHashCode() ^
                        this._promoteMessageCode.GetHashCode() ^
                        this._promoteMessageParams.GetHashCode();

            return hashCode;
        }

        #endregion

        #region Private Methods

        private void LoadEntityHistoryRecord(String valuesAsXml)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null);

                while (!reader.EOF)
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "EntityHistoryRecord")
                    {
                        #region Read EntityHistoryRecord Properties

                        if (reader.HasAttributes)
                        {
                            if (reader.MoveToAttribute("ChangeType"))
                            {
                                EntityChangeType entityChangeType = EntityChangeType.Unknown;
                                Enum.TryParse<EntityChangeType>(reader.ReadContentAsString(), out entityChangeType);
                                this.ChangeType = entityChangeType;
                            }

                            if (reader.MoveToAttribute("EntityLongName"))
                            {
                                this.ChangedData_EntityLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CategoryId"))
                            {
                                Int64 changedData_CategoryId = -1;
                                Int64.TryParse(reader.ReadContentAsString(), out changedData_CategoryId);
                                this.ChangedData_CategoryId = changedData_CategoryId;
                            }

                            if (reader.MoveToAttribute("CategoryLongName"))
                            {
                                this.ChangedData_CategoryLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("CategoryLongNamePath"))
                            {
                                this.ChangedData_CategoryLongNamePath = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelatedEntityId"))
                            {
                                Int32 changedData_RelatedEntityId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_RelatedEntityId);
                                this.ChangedData_RelatedEntityId = changedData_RelatedEntityId;
                            }

                            if (reader.MoveToAttribute("RelatedEntityLongName"))
                            {
                                this.ChangedData_RelatedEntityLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ExtensionContainerId"))
                            {
                                Int32 changedData_ExtensionContainerId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_ExtensionContainerId);
                                this.ChangedData_ExtensionContainerId = changedData_ExtensionContainerId;
                            }

                            if (reader.MoveToAttribute("ExtensionContainerLongName"))
                            {
                                this.ChangedData_ExtensionContainerLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ExtensionCategoryId"))
                            {
                                Int32 changedData_ExtensionCategoryId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_ExtensionCategoryId);
                                this.ChangedData_ExtensionCategoryId = changedData_ExtensionCategoryId;
                            }

                            if (reader.MoveToAttribute("ExtensionCategoryLongName"))
                            {
                                this.ChangedData_ExtensionCategoryLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ExtensionCategoryLongNamePath"))
                            {
                                this.ChangedData_ExtensionCategoryLongNamePath = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelationshipTypeId"))
                            {
                                Int32 changedData_RelationshipTypeId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_RelationshipTypeId);
                                this.ChangedData_RelationshipTypeId = changedData_RelationshipTypeId;
                            }

                            if (reader.MoveToAttribute("RelationshipTypeLongName"))
                            {
                                this.ChangedData_RelationshipTypeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelatedEntityTypeId"))
                            {
                                Int32 changedData_RelatedEntityTypeId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_RelatedEntityTypeId);
                                this.ChangedData_RelatedEntityTypeId = changedData_RelatedEntityTypeId;
                            }

                            if (reader.MoveToAttribute("RelatedEntityTypeLongName"))
                            {
                                this.ChangedData_RelatedEntityTypeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelatedEntityParentEntityTypeLongName"))
                            {
                                this.ChangedData_RelatedEntityParentEntityTypeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelatedEntityParentLongName"))
                            {
                                this.ChangedData_RelatedEntityParentLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("RelatedEntityParentId"))
                            {
                                Int64 changedData_RelatedEntityParentId = -1;
                                Int64.TryParse(reader.ReadContentAsString(), out changedData_RelatedEntityParentId);
                                this.ChangedData_RelatedEntityParentId = changedData_RelatedEntityParentId;
                            }

                            if (reader.MoveToAttribute("AttributeId"))
                            {
                                Int32 changedData_AttributeId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_AttributeId);
                                this.ChangedData_AttributeId = changedData_AttributeId;
                            }

                            if (reader.MoveToAttribute("AttributeLongName"))
                            {
                                this.ChangedData_AttributeLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AttributeParentLongName"))
                            {
                                this.ChangedData_AttributeParentLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("UOMId"))
                            {
                                Int32 changedData_UOMId = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_UOMId);
                                this.ChangedData_UOMId = changedData_UOMId;
                            }

                            if (reader.MoveToAttribute("UOM"))
                            {
                                this.ChangedData_UOM = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Sequence"))
                            {
                                Int32 changedData_Seq = -1;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_Seq);
                                this.ChangedData_Seq = changedData_Seq;
                            }

                            if (reader.MoveToAttribute("InstanceRefId"))
                            {
                                Int32 changedData_InstanceRefId;
                                Int32.TryParse(reader.ReadContentAsString(), out changedData_InstanceRefId);
                                this.ChangedData_InstanceRefId = changedData_InstanceRefId;
                            }

                            if (reader.MoveToAttribute("AttributeValue"))
                            {
                                this.ChangedData_AttrVal = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("PreviousValue"))
                            {
                                this.PreviousVal = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Source"))
							{
								this.Source = reader.ReadContentAsString();
							}

                            if (reader.MoveToAttribute("PreviousSource"))
                            {
                                this.PreviousSource = reader.ReadContentAsString();
                            }
							if (reader.MoveToAttribute("WorkflowName"))
                            {
                                this.WorkflowName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowVersionName"))
                            {
                                this.WorkflowVersionName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowRuntimeInstanceId"))
                            {
                                this.WorkflowRuntimeInstanceId= reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowActivityLongName"))
                            {
                                this.WorkflowActivityLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowActivityActionTaken"))
                            {
                                this.WorkflowActivityActionTaken = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowComments"))
                            {
                                this.WorkflowComments = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowPreviousAssignedUser"))
                            {
                                this.WorkflowPreviousAssignedUser = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("WorkflowCurrentAssignedUser"))
                            {
                                this.WorkflowCurrentAssignedUser = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("Details"))
                            {
                                this.Details = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("IsInvalidData"))
                            {
                                this.IsInvalidData = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), false);
                            }

                            if (reader.MoveToAttribute("ModifiedDate"))
                            {
                                this.ModifiedDateTime = ValueTypeHelper.ConvertToNullableDateTime(reader.ReadContentAsString());
                            }

                            if (reader.MoveToAttribute("ModifiedUser"))
                            {
                                this.ModifiedUser = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("ModifiedProgram"))
                            {
                                this.ModifiedProgram = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("AuditRefId"))
                            {
                                Int64 auditrefId = -1;
                                Int64.TryParse(reader.ReadContentAsString(),out auditrefId);
                                this.AuditRefId = auditrefId;
                            }

                            if (reader.MoveToAttribute("Action"))
                            {
                                ObjectAction action = ObjectAction.Unknown;
                                Enum.TryParse<ObjectAction>(reader.ReadContentAsString(), out action);
                                this.Action = action;
                            }

                            if (reader.MoveToAttribute("ModifiedDay"))
                            {
                                this.ModifiedDay = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("PromotedRootEntityId"))
                            {
                                Int64 promotedRootEntityId = -1;
                                Int64.TryParse(reader.ReadContentAsString(), out promotedRootEntityId);
                                this.PromotedRootEntityId = promotedRootEntityId;
                            }

                            if (reader.MoveToAttribute("PromotedRootEntityLongName"))
                            {
                                this.PromotedRootEntityLongName = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("PromotedAttributesString"))
                            {
                                this.PromotedAttributesString = reader.ReadContentAsString();
                            }
                            
                            if (reader.MoveToAttribute("PromoteMessageCode"))
                            {
                                this.PromoteMessageCode = reader.ReadContentAsString();
                            }

                            if (reader.MoveToAttribute("PromoteMessageParams"))
                            {
                                this.PromoteMessageParams = reader.ReadContentAsString();
                            }
                            if (reader.MoveToAttribute("PromoteRootEntityTypeLongName"))
                            {
                                this.PromoteRootEntityTypeLongName = reader.ReadContentAsString();
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

        #endregion

        #endregion
    }
}
