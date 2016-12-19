using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRule Mapping information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(MDMRuleMapRuleCollection))]
    public class MDMRuleMap : MDMObject, IMDMRuleMap, ICloneable, IBusinessRuleObject
    {
        #region Fields

        /// <summary>
        /// Field denotes the reference Id
        /// </summary>
        private Int64 _referenceId = -1;

        /// <summary>
        /// Field for event id
        /// </summary>
        private Int32 _eventId;

        /// <summary>
        /// Field for event name
        /// </summary>
        private String _eventName;

        /// <summary>
        /// Field denotes the event type
        /// </summary>
        private MDMEventType _eventType;

        /// <summary>
        /// Field for application context
        /// </summary>
        private ApplicationContext _applicationContext;

        /// <summary>
        /// Field for value which indicates whether rule should be executed async or not
        /// </summary>
        private Boolean _isAsyncRule = false;

        /// <summary>
        /// Field for value which indicates whether rule mapped under the current context and event is enabled or not
        /// </summary>
        private Boolean _isEnabled = true;

        /// <summary>
        /// Field for workflow related information
        /// </summary>
        private WorkflowInfo _workflowInfo = null;

        /// <summary>
        /// 
        /// </summary>
        private MDMRuleMapRuleCollection _mdmRuleMapRules = null;

        /// <summary>
        /// Field denoting the audit info
        /// </summary>
        private AuditInfo _auditInfo = null;

        /// <summary>
        /// Field denoting the original MDMRuleMap
        /// </summary>
        private MDMRuleMap _originalMdMRuleMap = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes the Reference Id
        /// </summary>
        [DataMember]
        public new Int64 ReferenceId
        {
            get
            {
                return _referenceId;
            }
            set
            {
                _referenceId = value;
            }
        }

        /// <summary>
        /// Indicates the event id
        /// </summary>
        [DataMember]
        public Int32 EventId
        {
            get { return _eventId; }
            set { _eventId = value; }
        }

        /// <summary>
        /// Property denotes the MDMEvent Type
        /// </summary>
        [DataMember]
        public MDMEventType EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        /// <summary>
        /// Indicates the name of the event
        /// </summary>
        [DataMember]
        public String EventName
        {
            get { return _eventName; }
            set { _eventName = value; }
        }

        /// <summary>
        /// Indicates the application context
        /// </summary>
        [DataMember]
        public ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = new ApplicationContext(5);    // 5 ==> tb_ObjectType table's DDG entry. Need to see alternative as Enum for this.
                }

                return _applicationContext;
            }
            set
            {
                _applicationContext = value;
            }
        }

        /// <summary>
        /// Indicates whether rule mapped under the current context and event is enabled or not
        /// </summary>
        [DataMember]
        public Boolean IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// Indicates whether rule should be executed async or not
        /// </summary>
        [DataMember]
        public Boolean IsAsyncRule
        {
            get { return _isAsyncRule; }
            set { _isAsyncRule = value; }
        }

        /// <summary>
        /// Indicates the workflow related information
        /// </summary>
        [DataMember]
        public WorkflowInfo WorkflowInfo
        {
            get
            {

                if (_workflowInfo == null)
                {
                    _workflowInfo = new WorkflowInfo();
                }
                return _workflowInfo;

            }
            set { _workflowInfo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public MDMRuleMapRuleCollection MDMRuleMapRules
        {
            get
            {
                if (this._mdmRuleMapRules == null)
                {
                    return this._mdmRuleMapRules = new MDMRuleMapRuleCollection();
                }

                return _mdmRuleMapRules;
            }
            set
            {
                this._mdmRuleMapRules = value;
            }
        }

        /// <summary>
        /// Property denoting the auditinfo
        /// </summary>
        [DataMember]
        public AuditInfo AuditInfo
        {
            get
            {
                if (_auditInfo == null)
                {
                    _auditInfo = new AuditInfo();
                }

                return _auditInfo;
            }
            set { _auditInfo = value; }
        }

        /// <summary>
        /// Field denoting the original MDMRuleMap
        /// </summary>
        public MDMRuleMap OriginalMdMRuleMap
        {
            get { return _originalMdMRuleMap; }
            set { _originalMdMRuleMap = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// ParameterLess Constructor
        /// </summary>
        public MDMRuleMap()
            : base()
        {

        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule object</param>
        public MDMRuleMap(String valuesAsXml)
        {
            LoadMDMRuleMapFromXml(valuesAsXml);
        }

        #endregion Constructor

        #region Methods

        #region Public Methods

        #region Override Methods

        /// <summary>
        /// Gets Xml representation for MDMRuleMap
        /// </summary>
        public override String ToXml()
        {
            #region Sample Xml

            /*
             <MDMRuleMap Id="" ReferenceId="" Name="" EventId="" EventName="" EventSource="" IsEnabled="" IsAsyncRule="" WorkflowName="" WorkflowActivityId="" 
	                     WorkflowActivityName="" WorkflowActivityLongName="" WorkflowAction="" Action="">
	                    <ApplicationContext Id="" Name="" LongName="" OrganizationId="" OrganizationName="" OrganizationLongName="" ContainerId="" ContainerName="" ContainerLongName=""
			                        EntityTypeId="" EntityTypeName="" EntityTypeLongName="" RelationshipTypeId="" RelationshipTypeName="" RelationshipTypeLongName="" 
			                        CategoryId="" CategoryName="" CategoryLongName="" CategoryPath="" EntityId="" EntityName="" EntityLongName=""
			                        AttributeId="" AttributeName="" AttributeLongName="" RoleId="" RoleName="" RoleLongName="" Locale="" UserId="" ContextType="" ReferenceId="">
	                    </ApplicationContext>
	                    <BusinessRules>
		                    <KeyValuePairItems>
			                    <KeyValuePairItem Key="" Value="">
		                    </KeyValuePairItems>
	                    </BusinessRules>
	                    <BusinessConditions>
		                    <KeyValuePairItems>
			                    <KeyValuePairItem Key="" Value="">
		                    </KeyValuePairItems>
	                    </BusinessConditions>
               </MDMRuleMap>
            */

            #endregion Sample Xml

            String output = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //MDMRuleMap node start
                    xmlWriter.WriteStartElement("MDMRuleMap");

                    xmlWriter.WriteAttributeString("Id", Id.ToString());
                    xmlWriter.WriteAttributeString("ReferenceId", ReferenceId.ToString());
                    xmlWriter.WriteAttributeString("Name", Name);

                    xmlWriter.WriteAttributeString("EventId", EventId.ToString());
                    xmlWriter.WriteAttributeString("EventName", EventName);
                    xmlWriter.WriteAttributeString("EventSource", EventType.ToString());


                    xmlWriter.WriteAttributeString("IsEnabled", IsEnabled.ToString());
                    xmlWriter.WriteAttributeString("IsAsyncRule", IsAsyncRule.ToString());

                    xmlWriter.WriteAttributeString("WorkflowId", WorkflowInfo.WorkflowId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowName", WorkflowInfo.WorkflowName);
                    xmlWriter.WriteAttributeString("WorkflowActivityId", WorkflowInfo.WorkflowActivityId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowActivityName", WorkflowInfo.WorkflowActivityShortName);
                    xmlWriter.WriteAttributeString("WorkflowActivityLongName", WorkflowInfo.WorkflowActivityLongName);
                    xmlWriter.WriteAttributeString("WorkflowActionId", WorkflowInfo.WorkflowActivityActionId.ToString());
                    xmlWriter.WriteAttributeString("WorkflowAction", WorkflowInfo.WorkflowActivityAction);

                    xmlWriter.WriteAttributeString("Action", Action.ToString());

                    if (this._applicationContext != null)
                    {
                        xmlWriter.WriteRaw(this.ApplicationContext.ToXml());
                    }

                    xmlWriter.WriteStartElement("MDMRuleMapRules");

                    if (this.MDMRuleMapRules != null)
                    {
                        xmlWriter.WriteRaw(this.MDMRuleMapRules.ToXml());
                    }

                    xmlWriter.WriteEndElement();


                    //MDMRuleMap node end
                    xmlWriter.WriteEndElement();
                }

                //Get the actual XML
                output = sw.ToString();
            }

            return output;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRuleMap object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRuleMap)
            {
                MDMRuleMap objectToBeCompared = obj as MDMRuleMap;

                if (!base.Equals(objectToBeCompared))
                {
                    return false;
                }

                if (this.IsEnabled != objectToBeCompared.IsEnabled)
                {
                    return false;
                }

                if (this.EventId != objectToBeCompared.EventId)
                {
                    return false;
                }

                if (String.Compare(this.EventName, objectToBeCompared.EventName) != 0)
                {
                    return false;
                }

                if (this.EventType != objectToBeCompared.EventType)
                {
                    return false;
                }

                if (!this.ApplicationContext.Equals(objectToBeCompared.ApplicationContext))
                {
                    return false;
                }

                if (this.IsAsyncRule != objectToBeCompared.IsAsyncRule)
                {
                    return false;
                }

                if ((this.WorkflowInfo != null && objectToBeCompared.WorkflowInfo == null) ||
                    (this.WorkflowInfo == null && objectToBeCompared.WorkflowInfo != null) ||
                    String.Compare(this.WorkflowInfo.WorkflowName, objectToBeCompared.WorkflowInfo.WorkflowName) != 0 ||
                    String.Compare(this.WorkflowInfo.WorkflowActivityShortName, objectToBeCompared.WorkflowInfo.WorkflowActivityShortName) != 0 ||
                    String.Compare(this.WorkflowInfo.WorkflowActivityAction, objectToBeCompared.WorkflowInfo.WorkflowActivityAction) != 0)
                {
                    return false;
                }

                if (!this.MDMRuleMapRules.Equals(objectToBeCompared.MDMRuleMapRules))
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
        public override Int32 GetHashCode()
        {
            int hashCode = 0;

            hashCode = base.GetHashCode() ^ this.ReferenceId.GetHashCode() ^ this.EventId.GetHashCode() ^ this.EventType.GetHashCode() ^ this.IsAsyncRule.GetHashCode() ^
                       this.IsEnabled.GetHashCode();

            if (!String.IsNullOrWhiteSpace(this.EventName))
            {
                hashCode = hashCode ^ this.EventName.GetHashCode();
            }

            if (this.ApplicationContext != null)
            {
                hashCode = hashCode ^ this.ApplicationContext.GetHashCode();
            }

            if (this.WorkflowInfo != null)
            {
                hashCode = hashCode ^ this.WorkflowInfo.GetHashCode();
            }

            if (this.MDMRuleMapRules != null)
            {
                hashCode = hashCode ^ this.MDMRuleMapRules.GetHashCode();
            }

            return hashCode;
        }

        #endregion Override Methods

        /// <summary>
        /// Compares MDMRuleMap object with current MDMRuleMap object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subsetMDMRuleMap">Indicates MDMRuleMap object to be compared with current MDMRuleMap object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMRuleMap subsetMDMRuleMap, Boolean compareIds = false)
        {
            if (compareIds)
            {
                if (this.EventId != subsetMDMRuleMap.EventId)
                {
                    return false;
                }

                if (this.EventType != subsetMDMRuleMap.EventType)
                {
                    return false;
                }

                if (this.WorkflowInfo.WorkflowActivityId != subsetMDMRuleMap.WorkflowInfo.WorkflowActivityId)
                {
                    return false;
                }
            }

            if (!base.IsSuperSetOf(subsetMDMRuleMap, compareIds))
            {
                return false;
            }

            if (String.Compare(this.Name, subsetMDMRuleMap.Name) != 0)
            {
                return false;
            }

            if (String.Compare(this.EventName, subsetMDMRuleMap.EventName) != 0)
            {
                return false;
            }

            if (this.EventType != subsetMDMRuleMap.EventType)
            {
                return false;
            }

            if (!this.ApplicationContext.IsSuperSetOf(subsetMDMRuleMap.ApplicationContext))
            {
                return false;
            }

            if (this.IsEnabled != subsetMDMRuleMap.IsEnabled)
            {
                return false;
            }

            if (this.IsAsyncRule != subsetMDMRuleMap.IsAsyncRule)
            {
                return false;
            }

            if (String.Compare(this.WorkflowInfo.WorkflowName, subsetMDMRuleMap.WorkflowInfo.WorkflowName) != 0)
            {
                return false;
            }

            if (String.Compare(this.WorkflowInfo.WorkflowActivityShortName, subsetMDMRuleMap.WorkflowInfo.WorkflowActivityShortName) != 0)
            {
                return false;
            }

            if (String.Compare(this.WorkflowInfo.WorkflowActivityLongName, subsetMDMRuleMap.WorkflowInfo.WorkflowActivityLongName) != 0)
            {
                return false;
            }

            if (String.Compare(this.WorkflowInfo.WorkflowActivityAction, subsetMDMRuleMap.WorkflowInfo.WorkflowActivityAction) != 0)
            {
                return false;
            }

            if (this.MDMRuleMapRules.Equals(subsetMDMRuleMap.MDMRuleMapRules))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the WorkflowInfo in MDMRule Map
        /// </summary>
        /// <returns>WorkflowInfo</returns>
        public IWorkflowInfo GetWorkflowInfo()
        {
            return this._workflowInfo;
        }

        /// <summary>
        /// Set the WorkflowInfo in MDMRule Map
        /// </summary>
        /// <param name="workflowInfo">Indicates the WorkflowInfo</param>
        public void SetWorkflowInfo(IWorkflowInfo workflowInfo)
        {
            if (workflowInfo != null)
            {
                this._workflowInfo = (WorkflowInfo)workflowInfo;
            }
        }

        #region Mapped BusinessRules Utilties

        /// <summary>
        /// Gets the published business rules Ids mapped to an applicationcontext
        /// </summary>
        /// <returns>Returns collection of published businessrules Ids</returns>
        public Collection<Int32> GetPublishedBusinessRulesIds()
        {
            return GetMDMRuleIdsByParams(ruleStatus: RuleStatus.Published);
        }
        
        /// <summary>
        /// Gets the published business rules names mapped to an applicationcontext
        /// </summary>
        /// <returns>List of published businessrules names</returns>
        public Collection<String> GetPublishedBusinessRulesNames()
        {
            return GetMDMRuleNamesByParams(ruleStatus: RuleStatus.Published);
        }

        /// <summary>
        /// Gets the draft business rules names mapped to an applicationcontext
        /// </summary>
        /// <returns>List of draft businessrules names</returns>
        public Collection<String> GetDraftBusinessRulesNames()
        {
            return GetMDMRuleNamesByParams(ruleStatus: RuleStatus.Draft);
        }

        #endregion Mapped BusinessRules Utilties

        #region Mapped BusinsessConditions Utilities

        /// <summary>
        /// Gets the published business conditions mapped to an applicationcontext
        /// </summary>
        /// <returns>List of business conditions names</returns>
        public Collection<String> GetPublishedBusinessConditionsNames()
        {
            return GetMDMRuleNamesByParams(ruleStatus: RuleStatus.Published, ruleType: MDMRuleType.BusinessCondition);
        }

        /// <summary>
        /// Gets the published business conditions mapped to an applicationcontext
        /// </summary>
        /// <returns>List of business conditions names</returns>
        public Collection<String> GetDraftBusinessConditionsNames()
        {
            return GetMDMRuleNamesByParams(ruleStatus: RuleStatus.Draft, ruleType: MDMRuleType.BusinessCondition);
        }

        #endregion Mapped BusinsessConditions

        /// <summary>
        /// Get All Mapped BusinessRules And BusinessConditions Ids
        /// </summary>
        /// <returns>List of BusinessRules And BusinessConditions Ids</returns>
        public Collection<Int32> GetAllBusinessRulesAndBusinessConditionsIds()
        {
            return this.GetMDMRuleIdsByParams(ignoreRuleType: true, ignoreRuleStatus: true);
        }

        /// <summary>
        /// Get All Mapped BusinessRules And BusinessConditions Names
        /// </summary>
        /// <returns>List of BusinessRules And BusinessConditions Names</returns>
        public Collection<String> GetAllBusinessRulesAndBusinessConditionsNames()
        {
            return this.GetMDMRuleNamesByParams(ignoreRuleType: true, ignoreRuleStatus: true);
        }

        /// <summary>
        /// Get the application context of MDMRule map
        /// </summary>
        /// <returns>ApplicationContext</returns>
        public IApplicationContext GetApplicationContext()
        {
            return this.ApplicationContext;
        }

        /// <summary>
        /// Set the application context to MDMRule map
        /// </summary>
        /// <param name="applicationContext">Indicates the application context</param>
        public void SetApplicationContext(IApplicationContext applicationContext)
        {
            this._applicationContext = (ApplicationContext)applicationContext;
        }

        /// <summary>
        /// Get Last Modified AuditInfo
        /// </summary>
        /// <returns>AuditInfo</returns>
        public IAuditInfo GetLastModifiedAuditInfo()
        {
            if (this._auditInfo == null)
            {
                this._auditInfo = new AuditInfo();
            }

            return _auditInfo;
        }

        /// <summary>
        /// Get all Business Rules and Business Conditions mapped to a MapContext
        /// </summary>
        /// <returns>Returns Business Rules and Business Conditions</returns>
        public IMDMRuleMapRuleCollection GetMDMRuleMapRules()
        {
            return this.MDMRuleMapRules;
        }

        /// <summary>
        /// Gets the MDMRuleMapRule by rule name and rule status
        /// </summary>
        /// <param name="ruleName">Indicates the rule name</param>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>Returns the MDMRuleMaprule</returns>
        public IMDMRuleMapRule GetMDMRuleMapRule(String ruleName, RuleStatus ruleStatus)
        {
            MDMRuleMapRule result = null;

            if (!String.IsNullOrWhiteSpace(ruleName) && this._mdmRuleMapRules != null && this._mdmRuleMapRules.Count > 0)
            {
                foreach (MDMRuleMapRule ruleMapRule in this._mdmRuleMapRules)
                {
                    if (String.Compare(ruleName, ruleMapRule.RuleName, true) == 0 && ruleStatus == ruleMapRule.RuleStatus)
                    {
                        result = ruleMapRule;
                        break;
                    }

                }
            }
            return result;
        }

        #endregion Public Methods

        #region ICloneable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMap object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMap object</returns>
        public MDMRuleMap Clone()
        {
            MDMRuleMap clonedObject = new MDMRuleMap();

            clonedObject.Id = this.Id;
            clonedObject.ReferenceId = this.ReferenceId;
            clonedObject.Name = this.Name;
            clonedObject.Action = this.Action;
            clonedObject.ApplicationContext = this.ApplicationContext.Clone();
            clonedObject.EventId = this.EventId;
            clonedObject.EventName = this.EventName;
            clonedObject.EventType = this.EventType;
            clonedObject.IsAsyncRule = this.IsAsyncRule;
            clonedObject.IsEnabled = this.IsEnabled;
            clonedObject.WorkflowInfo.WorkflowId = this.WorkflowInfo.WorkflowId;
            clonedObject.WorkflowInfo.WorkflowName = this.WorkflowInfo.WorkflowName;
            clonedObject.WorkflowInfo.WorkflowActivityId = this.WorkflowInfo.WorkflowActivityId;
            clonedObject.WorkflowInfo.WorkflowActivityShortName = this.WorkflowInfo.WorkflowActivityShortName;
            clonedObject.WorkflowInfo.WorkflowActivityLongName = this.WorkflowInfo.WorkflowActivityLongName;
            clonedObject.WorkflowInfo.WorkflowActivityActionId = this.WorkflowInfo.WorkflowActivityActionId;
            clonedObject.WorkflowInfo.WorkflowActivityAction = this.WorkflowInfo.WorkflowActivityAction;
            clonedObject.MDMRuleMapRules = this.MDMRuleMapRules;
            clonedObject.AuditInfo = this.AuditInfo;

            return clonedObject;
        }

        /// <summary>
        /// Gets a cloned instance of the current MDMRuleMap object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRuleMap object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion ICloneable Members

        #region Private Methods

        private void LoadMDMRuleMapFromXml(String valuesAsXml)
        {
            #region Sample Xml

            /*
             <MDMRuleMap Id="" ReferenceId="" Name="" EventId="" EventName="" EventSource="" IsEnabled="" IsAsyncRule="" WorkflowName="" WorkflowActivityId="" 
	                     WorkflowActivityName="" WorkflowActivityLongName="" WorkflowAction="" Action="">
	                    <ApplicationContext Id="" Name="" LongName="" OrganizationId="" OrganizationName="" OrganizationLongName="" ContainerId="" ContainerName="" ContainerLongName=""
			                        EntityTypeId="" EntityTypeName="" EntityTypeLongName="" RelationshipTypeId="" RelationshipTypeName="" RelationshipTypeLongName="" 
			                        CategoryId="" CategoryName="" CategoryLongName="" CategoryPath="" EntityId="" EntityName="" EntityLongName=""
			                        AttributeId="" AttributeName="" AttributeLongName="" RoleId="" RoleName="" RoleLongName="" Locale="" UserId="" ContextType="" ReferenceId="">
	                    </ApplicationContext>
	                    <BusinessRules>
		                    <KeyValuePairItems>
			                    <KeyValuePairItem Key="" Value="">
		                    </KeyValuePairItems>
	                    </BusinessRules>
	                    <BusinessConditions>
		                    <KeyValuePairItems>
			                    <KeyValuePairItem Key="" Value="">
		                    </KeyValuePairItems>
	                    </BusinessConditions>
               </MDMRuleMap>
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    if (reader != null)
                    {
                        while (!reader.EOF)
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMap")
                            {
                                #region Read MDMRuleMap Attributes

                                if (reader.HasAttributes)
                                {
                                    if (reader.MoveToAttribute("Id"))
                                    {
                                        this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                    }

                                    if (reader.MoveToAttribute("ReferenceId"))
                                    {
                                        this.ReferenceId = ValueTypeHelper.Int64TryParse(reader.ReadContentAsString(), _referenceId);
                                    }

                                    if (reader.MoveToAttribute("Name"))
                                    {
                                        this.Name = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("EventId"))
                                    {
                                        this.EventId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.EventId);
                                    }

                                    if (reader.MoveToAttribute("EventName"))
                                    {
                                        this.EventName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("EventSource"))
                                    {
                                        ValueTypeHelper.EnumTryParse<MDMEventType>(reader.ReadContentAsString(), false, out this._eventType);
                                    }

                                    if (reader.MoveToAttribute("IsAsyncRule"))
                                    {
                                        this.IsAsyncRule = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsAsyncRule);
                                    }

                                    if (reader.MoveToAttribute("IsEnabled"))
                                    {
                                        this.IsEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsEnabled);
                                    }

                                    if (reader.MoveToAttribute("WorkflowId"))
                                    {
                                        this.WorkflowInfo.WorkflowId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowInfo.WorkflowId);
                                    }

                                    if (reader.MoveToAttribute("WorkflowName"))
                                    {
                                        this.WorkflowInfo.WorkflowName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowActivityId"))
                                    {
                                        this.WorkflowInfo.WorkflowActivityId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowInfo.WorkflowActivityId);
                                    }

                                    if (reader.MoveToAttribute("WorkflowActivityName"))
                                    {
                                        this.WorkflowInfo.WorkflowActivityShortName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowActivityLongName"))
                                    {
                                        this.WorkflowInfo.WorkflowActivityLongName = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("WorkflowActionId"))
                                    {
                                        this.WorkflowInfo.WorkflowActivityActionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.WorkflowInfo.WorkflowActivityActionId);
                                    }

                                    if (reader.MoveToAttribute("WorkflowAction"))
                                    {
                                        this.WorkflowInfo.WorkflowActivityAction = reader.ReadContentAsString();
                                    }

                                    if (reader.MoveToAttribute("Action"))
                                    {
                                        this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                    }

                                    reader.Read();
                                }

                                #endregion
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "ApplicationContext")
                            {
                                String applicationContextXml = reader.ReadOuterXml();

                                if (!String.IsNullOrWhiteSpace(applicationContextXml))
                                {
                                    this.ApplicationContext = new ApplicationContext(applicationContextXml, Constants.DDG_OBJECTTYPE_ID);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleMapRules")
                            {
                                String mdmRuleMapRulesAsXml = reader.ReadOuterXml();

                                if (!String.IsNullOrWhiteSpace(mdmRuleMapRulesAsXml))
                                {
                                    this.MDMRuleMapRules = new MDMRuleMapRuleCollection(mdmRuleMapRulesAsXml);
                                }
                            }
                            else
                            {
                                //Keep on reading the xml until we reach expected node.
                                reader.Read();
                            }
                        }
                    }
                }
            }
        }

        private Boolean CompareMappedRules(Dictionary<Int32, String> currentObject, Dictionary<Int32, String> originalObject)
        {
            Int32 currentObjCount = currentObject.Count;
            Int32 originalObjCount = originalObject.Count;

            if (currentObjCount != originalObjCount)
            {
                return false;
            }
            else
            {
                if (currentObjCount > 0 && originalObjCount > 0)
                {
                    Int32[] currentObjRules = currentObject.Keys.ToArray();
                    Int32[] originalObjRules = originalObject.Keys.ToArray();

                    for (Int32 index = 0; index < currentObjCount; index++)
                    {
                        if (currentObjRules[index] != originalObjRules[index])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private Collection<Int32> GetMDMRuleIds(Dictionary<Int32, String> mdmRules, Collection<Int32> mdmRuleIds = null)
        {
            Collection<Int32> result = new Collection<Int32>();

            if (mdmRules != null)
            {
                if (mdmRuleIds != null && mdmRuleIds.Count > 0)
                {
                    result = mdmRuleIds;
                }

                foreach (KeyValuePair<Int32, String> mdmRule in mdmRules)
                {
                    if (result.Contains(mdmRule.Key) == false)
                    {
                        result.Add(mdmRule.Key);
                    }
                }
            }

            return result;
        }

        private Collection<String> GetMDMRuleNames(Dictionary<Int32, String> mdmRules, Collection<String> mdmRuleNames = null)
        {
            Collection<String> result = new Collection<String>();

            if (mdmRules != null)
            {
                if (mdmRuleNames != null && mdmRuleNames.Count > 0)
                {
                    result = mdmRuleNames;
                }

                foreach (KeyValuePair<Int32, String> mdmRule in mdmRules)
                {
                    if (result.Contains(mdmRule.Value) == false)
                    {
                        result.Add(mdmRule.Value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="ruleStatus"></param>
        /// <param name="ignoreRuleType"></param>
        /// <param name="ignoreRuleStatus"></param>
        /// <returns></returns>
        private Collection<Int32> GetMDMRuleIdsByParams(RuleStatus ruleStatus = RuleStatus.Unknown, MDMRuleType ruleType = MDMRuleType.UnKnown, Boolean ignoreRuleType = false, Boolean ignoreRuleStatus = false)
        {
            Collection<Int32> ruleIds = new Collection<Int32>();

            if (this.MDMRuleMapRules != null)
            {
                foreach (MDMRuleMapRule ruleMapRule in this.MDMRuleMapRules)
                {
                    if ((ignoreRuleType && ignoreRuleStatus) ||
                        (ignoreRuleType && ruleStatus == ruleMapRule.RuleStatus) ||
                        (ignoreRuleStatus && (ruleType == ruleMapRule.RuleType || ruleType != MDMRuleType.BusinessCondition)) ||
                        (ruleStatus == ruleMapRule.RuleStatus && (ruleType == ruleMapRule.RuleType || ruleType != MDMRuleType.BusinessCondition)))
                    {
                        ruleIds.Add(ruleMapRule.RuleId);
                    }
                }
            }

            return ruleIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="ruleStatus"></param>
        /// <param name="ignoreRuleType"></param>
        /// <param name="ignoreRuleStatus"></param>
        /// <returns></returns>
        private Collection<String> GetMDMRuleNamesByParams(RuleStatus ruleStatus = RuleStatus.Unknown, MDMRuleType ruleType = MDMRuleType.UnKnown, Boolean ignoreRuleType = false, Boolean ignoreRuleStatus = false)
        {
            Collection<String> ruleNames = new Collection<String>();

            if (this.MDMRuleMapRules != null)
            {
                
                foreach (MDMRuleMapRule ruleMapRule in this.MDMRuleMapRules)
                {
                    if ((ignoreRuleType && ignoreRuleStatus) ||
                        (ignoreRuleType && ruleStatus == ruleMapRule.RuleStatus) ||
                        (ignoreRuleStatus && (ruleType == ruleMapRule.RuleType || ruleType != MDMRuleType.BusinessCondition)) ||
                        (ruleStatus == ruleMapRule.RuleStatus && (ruleType == ruleMapRule.RuleType || ruleType != MDMRuleType.BusinessCondition)))
                    {
                        ruleNames.Add(ruleMapRule.RuleName);
                    }
                }
            }

            return ruleNames;
        }

        #endregion Private Methods

        #endregion Methods
    }
}