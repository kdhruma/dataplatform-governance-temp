using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain MDMRule information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(MDMRuleContext))]
    [KnownType(typeof(MDMRuleDisplayList))]
    public class MDMRule : MDMObject, IMDMRule, ICloneable, IBusinessRuleObject
    {
        #region Fields

        /// <summary>
        /// Field denotes the reference Id
        /// </summary>
        private Int32 _referenceId = -1;

        /// <summary>
        /// Field for rule type
        /// </summary>
        private MDMRuleType _ruleType = MDMRuleType.UnKnown;

        /// <summary>
        /// Field for rule description
        /// </summary>
        private String _description;

        /// <summary>
        /// Field for rule definition
        /// </summary>
        private String _ruleDefinition;

        /// <summary>
        /// Field for rule context
        /// </summary>
        private MDMRuleContext _ruleContext;

        /// <summary>
        /// Field denoting the IDs of mapping to which rules are mapped
        /// </summary>
        private Collection<Int32> _ruleMapIds;

        /// <summary>
        /// Field denoting the names of mapping to which rules are mapped
        /// Todo.. DB Changes Required
        /// </summary>
        private Collection<String> _ruleMapNames;

        /// <summary>
        /// Field denoting the display type
        /// </summary>
        private DisplayType _displayType = DisplayType.Unknown;

        /// <summary>
        /// Field denoting the display list
        /// </summary>
        private MDMRuleDisplayList _displayList = null;

        /// <summary>
        /// Field denoting whether rule is a system rule
        /// </summary>
        private Boolean _isSystemRule = false;

        /// <summary>
        /// Field denoting whether rule is enabled or not
        /// </summary>
        private Boolean _isEnabled = true;

        /// <summary>
        /// Field denoting BusinessRule/BusinessCondition is published or draft
        /// </summary>
        private RuleStatus _ruleStatus = RuleStatus.Draft;

        /// <summary>
        /// Field denoting id of publish version of BusinessRule/BusinessCondition
        /// </summary>
        private Int32 _publishedVersionId = 0;

        /// <summary>
        /// Field denoting audit info
        /// </summary>
        private AuditInfo _auditInfo = null;

        /// <summary>
        /// Field denoting the original MDM rule
        /// </summary>
        private MDMRule _originalMdMRule = null;

        /// <summary>
        /// Field denoting the BusinessCondition rules.
        /// </summary>
        private MDMRuleCollection _businessConditionRules = null;

        /// <summary>
        /// Field denoting the target attribute name
        /// </summary>
        private String _targetAttributeName = String.Empty;

        /// <summary>
        /// Field denoting the target locale
        /// </summary>
        private LocaleEnum _targetLocale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field denoting the sequence in which the business rules will be executed
        /// </summary>
        private Int32 _sequence = Constants.DDG_DEFAULT_SORTORDER;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denoting the Reference Id
        /// </summary>
        [DataMember]
        public new Int32 ReferenceId
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
        /// Property denoting the rule type
        /// </summary>
        [DataMember]
        public MDMRuleType RuleType
        {
            get { return _ruleType; }
            set { _ruleType = value; }
        }

        /// <summary>
        /// Property denoting the rule description
        /// </summary>
        [DataMember]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Property denoting the rule definition
        /// </summary>
        [DataMember]
        public String RuleDefinition
        {
            get { return _ruleDefinition; }
            set { _ruleDefinition = value; }
        }

        /// <summary>
        /// Property denoting the rule context for the rule
        /// It will contain AttributeName and Locale in Xml
        /// </summary>
        [DataMember]
        public MDMRuleContext RuleContext
        {
            get
            {
                if (this._ruleContext == null)
                {
                    this._ruleContext = new MDMRuleContext();
                }
                return _ruleContext;
            }
            set
            {
                _ruleContext = value;
            }
        }

        /// <summary>
        /// Property denoting the ids of mapping to which rule are mapped
        /// </summary>
        [DataMember]
        public Collection<Int32> RuleMapIds
        {
            get
            {
                if (this._ruleMapIds == null)
                {
                    this._ruleMapIds = new Collection<Int32>();
                }
                return _ruleMapIds;
            }
            set { _ruleMapIds = value; }
        }

        /// <summary>
        /// Property denoting the names of mapping to which rule are mapped
        /// </summary>
        [DataMember]
        public Collection<String> RuleMapNames
        {
            get
            {
                if (this._ruleMapNames == null)
                {
                    this._ruleMapNames = new Collection<String>();
                }
                return _ruleMapNames;
            }
            set { _ruleMapNames = value; }
        }

        /// <summary>
        /// Property denoting the display type
        /// </summary>
        [DataMember]
        public DisplayType DisplayType
        {
            get { return _displayType; }
            set { _displayType = value; }
        }

        /// <summary>
        /// Property denoting the display list
        /// </summary>
        [DataMember]
        public MDMRuleDisplayList DisplayList
        {
            get
            {
                if (_displayList == null)
                {
                    _displayList = new MDMRuleDisplayList(this._displayType);
                }

                return _displayList;
            }
            set { _displayList = value; }
        }

        /// <summary>
        /// Property denoting whether rule is a system rule
        /// </summary>
        [DataMember]
        public Boolean IsSystemRule
        {
            get { return _isSystemRule; }
            set { _isSystemRule = value; }
        }

        /// <summary>
        /// Property denoting whether the rule is enabled
        /// </summary>
        [DataMember]
        public Boolean IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        /// <summary>
        /// Property denoting BusinessRule/BusinessCondition is published or draft
        /// </summary>
        [DataMember]
        public RuleStatus RuleStatus
        {
            get { return _ruleStatus; }
            set { _ruleStatus = value; }
        }

        /// <summary>
        /// Property denoting id of published version of BusinessRule/BusinessCondition
        /// </summary>
        [DataMember]
        public Int32 PublishedVersionId
        {
            get { return _publishedVersionId; }
            set { _publishedVersionId = value; }
        }

        /// <summary>
        /// Property denoting BusinessConditionRules
        /// </summary>
        [DataMember]
        public MDMRuleCollection BusinessConditionRules
        {
            get
            {
                if (this._businessConditionRules == null)
                {
                    this._businessConditionRules = new MDMRuleCollection();
                }

                return _businessConditionRules;
            }
            set
            {
                _businessConditionRules = value;
            }
        }

        /// <summary>
        /// Property denoting the All BusinessConditionRule Ids
        /// </summary>
        [DataMember]
        public Collection<Int32> BusinessConditionRuleIds
        {
            get
            {
                return GetBusinessConditionRuleIds(RuleStatus.Unknown) as Collection<Int32>;
            }
        }

        /// <summary>
        /// Property denoting the Published BusinessConditionRule Ids
        /// </summary>
        [DataMember]
        public Collection<Int32> PublishedBusinessConditionRuleIds
        {
            get
            {
                return GetBusinessConditionRuleIds(RuleStatus.Published) as Collection<Int32>;
            }
        }

        /// <summary>
        /// Property denoting the Draft BusinessConditionRule Ids
        /// </summary>
        [DataMember]
        public Collection<Int32> DraftBusinessConditionRuleIds
        {
            get
            {
                return GetBusinessConditionRuleIds(RuleStatus.Draft) as Collection<Int32>;
            }
        }

        /// <summary>
        /// Property denoting the All BusinessConditionRule Names
        /// </summary>
        [DataMember]
        public Collection<String> BusinessConditionRuleNames
        {
            get
            {
                return GetBusinessConditionRuleNames(RuleStatus.Unknown) as Collection<String>;
            }
        }

        /// <summary>
        /// Property denoting the Publish BusinessConditionRule Names
        /// </summary>
        [DataMember]
        public Collection<String> PublishedBusinessConditionRuleNames
        {
            get
            {
                return GetBusinessConditionRuleNames(RuleStatus.Published) as Collection<String>;
            }
        }

        /// <summary>
        /// Property denoting the Draft BusinessConditionRule Names
        /// </summary>
        [DataMember]
        public Collection<String> DraftBusinessConditionRuleNames
        {
            get
            {
                return GetBusinessConditionRuleNames(RuleStatus.Draft) as Collection<String>;
            }
        }

        /// <summary>
        /// Property denoting audit info
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
        /// Property denoting the Original MDM Rule
        /// </summary>
        public MDMRule OriginalMdMRule
        {
            get { return _originalMdMRule; }
            set { _originalMdMRule = value; }
        }

        /// <summary>
        /// Property denoting the target attribute name
        /// </summary>
        [DataMember]
        public String TargetAttributeName
        {
            get { return _targetAttributeName; }
            set { _targetAttributeName = value; }
        }

        /// <summary>
        /// Property denoting the target locale
        /// </summary>
        [DataMember]
        public LocaleEnum TargetLocale
        {
            get { return _targetLocale; }
            set { _targetLocale = value; }
        }

        /// <summary>
        /// Property denoting the sequence in which the business rules will be executed
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public MDMRule()
            : base()
        {

        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="mdmRuleName">Indicates the rule name</param>
        /// <param name="mdmRuleStatus">Indicates the rule status</param>
        public MDMRule(String mdmRuleName, RuleStatus mdmRuleStatus = RuleStatus.Draft)
            : base()
        {
            this.Name = mdmRuleName;
            this.RuleStatus = mdmRuleStatus;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="mdmRuleId">Indicates the rule id</param>
        /// <param name="mdmRuleName">Indicates the rule name</param>
        /// <param name="mdmRuleStatus">Indicates the rule status</param>
        /// <param name="sequence">Indicates the sequence in which the business condition rule should be executed</param>
        public MDMRule(Int32 mdmRuleId, String mdmRuleName, RuleStatus mdmRuleStatus = RuleStatus.Draft, Int32 sequence = Constants.DDG_DEFAULT_SORTORDER)
            : base()
        {
            this.Id = mdmRuleId;
            this.Name = mdmRuleName;
            this.RuleStatus = mdmRuleStatus;
            this.Sequence = sequence;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="mdmRuleName">Indicates the rule name</param>
        /// <param name="mdmRuleType">Indicates the rule type</param>
        /// <param name="mdmRuleStatus">Indicates the rule status</param>
        public MDMRule(String mdmRuleName, MDMRuleType mdmRuleType = MDMRuleType.Governance, RuleStatus mdmRuleStatus = RuleStatus.Draft)
            : base()
        {
            this.Name = mdmRuleName;
            this.RuleType = mdmRuleType;
            this.RuleStatus = mdmRuleStatus;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="mdmRuleId">Indicates the rule id</param>
        /// <param name="mdmRuleName">Indicates the rule name</param>
        /// <param name="mdmRuleType">Indicates the rule type</param>
        /// <param name="mdmRuleStatus">Indicates the rule status</param>
        public MDMRule(Int32 mdmRuleId, String mdmRuleName, MDMRuleType mdmRuleType = MDMRuleType.Governance, RuleStatus mdmRuleStatus = RuleStatus.Draft)
            : base()
        {
            this.Id = mdmRuleId;
            this.Name = mdmRuleName;
            this.RuleType = mdmRuleType;
            this.RuleStatus = mdmRuleStatus;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="mdmRuleId">Indicates the rule id</param>
        /// <param name="mdmRuleName">Indicates the rule name</param>
        /// <param name="mdmRuleType">Indicates the rule type</param>
        /// <param name="mdmRuleStatus">Indicates the rule status</param>
        /// <param name="sequence">Indicates the sequence</param>
        public MDMRule(Int32 mdmRuleId, String mdmRuleName, MDMRuleType mdmRuleType = MDMRuleType.Governance, RuleStatus mdmRuleStatus = RuleStatus.Draft, Int32 sequence = Constants.DDG_DEFAULT_SORTORDER)
            : base()
        {
            this.Id = mdmRuleId;
            this.Name = mdmRuleName;
            this.RuleType = mdmRuleType;
            this.RuleStatus = mdmRuleStatus;
            this.Sequence = sequence;
        }

        /// <summary>
        /// Parameterised constructor having object values in xml format.
        /// Populates current object using XML.
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule object</param>
        public MDMRule(String valuesAsXml)
        {
            LoadMDMRuleFromXml(valuesAsXml);
        }

        #endregion Constructors

        #region Methods

        #region Public Methods

        #region Clone & Merge Delta

        /// <summary>
        /// Gets a cloned instance of the current MDMRule object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRule object</returns>
        public MDMRule Clone()
        {
            MDMRule clonedMdmRule = new MDMRule();

            clonedMdmRule.Id = this.Id;
            clonedMdmRule.ReferenceId = this.ReferenceId;
            clonedMdmRule.Name = this.Name;
            clonedMdmRule.RuleType = this.RuleType;
            clonedMdmRule.Description = this.Description;
            clonedMdmRule.RuleDefinition = this.RuleDefinition;
            clonedMdmRule.RuleContext = this.RuleContext;
            clonedMdmRule.RuleMapIds = this.RuleMapIds;
            clonedMdmRule.RuleMapNames = this.RuleMapNames;
            clonedMdmRule.DisplayType = this.DisplayType;
            clonedMdmRule.DisplayList = this.DisplayList;
            clonedMdmRule.IsSystemRule = this.IsSystemRule;
            clonedMdmRule.IsEnabled = this.IsEnabled;
            clonedMdmRule.RuleStatus = this.RuleStatus;
            clonedMdmRule.PublishedVersionId = this.PublishedVersionId;
            clonedMdmRule.AuditInfo = this.AuditInfo;
            clonedMdmRule.Action = this.Action;
            clonedMdmRule.BusinessConditionRules = this.BusinessConditionRules;
            clonedMdmRule.TargetAttributeName = this.TargetAttributeName;
            clonedMdmRule.TargetLocale = this.TargetLocale;
            clonedMdmRule.Sequence = this.Sequence;

            return clonedMdmRule;
        }

        /// <summary>
        /// Delta Merge of MDMRule
        /// </summary>
        /// <param name="deltaMdmRule">>Entity Type that needs to be merged</param>
        /// <param name="returnClonedObject">True means return new merged cloned object;otherwise, same object</param>
        /// <param name="withContextMapping">Indicates that Rule have populated context mapping collections (RuleMapIds and RuleMapNames)</param>
        /// <returns>Merged MDMRule</returns>
        public MDMRule MergeDelta(MDMRule deltaMdmRule, Boolean returnClonedObject = true, Boolean withContextMapping = false)
        {
            MDMRule mergedMdmRule = returnClonedObject ? deltaMdmRule.Clone() : deltaMdmRule;

            Boolean isEquals = withContextMapping ? mergedMdmRule.EqualsWithContextMapping(this) : mergedMdmRule.Equals(this);
            mergedMdmRule.Action = isEquals ? ObjectAction.Read : ObjectAction.Update;

            return mergedMdmRule;
        }

        #endregion Clone & Merge Delta

        #region Interface Methods

        /// <summary>
        /// Sets the rule context to MDMRule
        /// </summary>
        /// <param name="ruleContext">Indicates the Rule context</param>
        public void SetRuleContext(IMDMRuleContext ruleContext)
        {
            if (ruleContext != null)
            {
                this.RuleContext = (MDMRuleContext)ruleContext;
            }
            else
            {
                throw new ArgumentNullException("ruleContext", "ruleContext can not be null");
            }
        }

        /// <summary>
        /// Returns list of Attributes or RelationshipTypes
        /// </summary>
        public IMDMRuleDisplayList GetDisplayList()
        {
            return this.DisplayList;
        }

        /// <summary>
        /// Sets the list of Attributes or RelationshipTypes to be displayed
        /// </summary>
        /// <param name="displayList">List of MDMObjects</param>
        public void SetDisplayList(IMDMRuleDisplayList displayList)
        {
            this._displayList = displayList as MDMRuleDisplayList;
        }

        /// <summary>
        /// Gets the business rules mapped to businesscondition
        /// </summary>
        /// <returns>BusinessConditionRules</returns>
        public IMDMRuleCollection GetBusinessConditionRules()
        {
            return this.BusinessConditionRules;
        }

        /// <summary>
        /// Get all BusinessConditionRule Ids based on rule status
        /// </summary>
        /// <param name="ruleStatus">Indicates the rule status</param>
        /// <returns>List of BusinessConditionRule Ids</returns>
        public ICollection<Int32> GetBusinessConditionRuleIds(RuleStatus ruleStatus)
        {
            return this.BusinessConditionRules.GetMDMRuleIds(ruleStatus);
        }

        /// <summary>
        /// Get all BusinessConditionRule Names
        /// </summary>
        /// <param name="ruleStatus">Indicates the ruleStatus</param>
        /// <returns>List of BusinessConditionRule Names</returns>
        public ICollection<String> GetBusinessConditionRuleNames(RuleStatus ruleStatus)
        {
            return this.BusinessConditionRules.GetMDMRuleNames(ruleStatus);
        }

        /// <summary>
        /// Add businessconditionrule name into MDMRule
        /// </summary>
        /// <param name="businessConditionRuleNames">Indicates the name of businessConditionRules</param>
        /// <param name="ruleStatus">Indicates the ruleStatus</param>
        public void SetBusinessConditionRules(ICollection<String> businessConditionRuleNames, RuleStatus ruleStatus)
        {
            if (businessConditionRuleNames != null)
            {
                Int32 tempRuleId = this.BusinessConditionRules.Count;

                foreach (String businessConditionRuleName in businessConditionRuleNames)
                {
                    this.SetBusinessConditionRule(-++tempRuleId, businessConditionRuleName, ruleStatus);
                }
            }
        }

        /// <summary>
        /// Add business condition rule name into MDMRule
        /// </summary>
        /// <param name="businessConditionRuleId">Indicates the business condition rule id</param>
        /// <param name="businessConditionRuleName">Indicates the business condition rule name</param>
        /// <param name="ruleStatus">Indicates the ruleStatus</param>
        /// <param name="sequence">Indicates the sequence in which the business condition rule has to be executed</param>
        public void SetBusinessConditionRule(Int32 businessConditionRuleId, String businessConditionRuleName, RuleStatus ruleStatus, Int32 sequence = Constants.DDG_DEFAULT_SORTORDER)
        {
            if (!String.IsNullOrWhiteSpace(businessConditionRuleName))
            {
                Int32 tempRuleId = this.BusinessConditionRules.Count;

                if (businessConditionRuleId <= 0)
                {
                    tempRuleId = -++tempRuleId;  //rule id would be the negative number for the reference when rule Id not passed to this method
                }
                else
                {
                    tempRuleId = businessConditionRuleId;
                }

                MDMRule businessConditionRule = new MDMRule(tempRuleId, businessConditionRuleName, ruleStatus, sequence);
                this.BusinessConditionRules.Add(businessConditionRule);
            }
        }

        /// <summary>
        /// Returns last modified AuditInfo
        /// </summary>
        /// <returns>AuditInfo</returns>
        public IAuditInfo GetLastModifiedAuditInfo()
        {
            if (this._auditInfo == null)
            {
                _auditInfo = new AuditInfo();
            }

            return _auditInfo;
        }

        /// <summary>
        /// Returns last published AuditInfo
        /// </summary>
        /// <returns>AuditInfo</returns>
        public IAuditInfo GetPublishAuditInfo()
        {
            if (this._auditInfo == null)
            {
                _auditInfo = new AuditInfo();
            }

            return _auditInfo;
        }

        #endregion Interface Methods

        #region Utility Mehtods

        /// <summary>
        /// Inserts rule map ids into rulemapid collection
        /// </summary>
        /// <param name="ruleMapIds">Indicates the collection of rule map ids</param>
        public void AddRuleMaps(Collection<Int32> ruleMapIds)
        {
            if (ruleMapIds != null && ruleMapIds.Count > 0)
            {
                foreach (Int32 ruleMapId in ruleMapIds)
                {
                    if (!this.RuleMapIds.Contains(ruleMapId))
                    {
                        this.RuleMapIds.Add(ruleMapId);
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object except IsEnabled and RuleStatus properties.
        /// </summary>
        /// <param name="objectToBeCompared">MDMRule object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public Boolean IsEqualsExceptState(MDMRule objectToBeCompared)
        {
            if (!base.Equals(objectToBeCompared))
            {
                return false;
            }

            if (this.RuleType != objectToBeCompared.RuleType)
            {
                return false;
            }

            if (String.Compare(this.Description, objectToBeCompared.Description) != 0)
            {
                return false;
            }

            if (!this.RuleContext.Equals(objectToBeCompared.RuleContext))
            {
                return false;
            }

            if (this.RuleType != MDMRuleType.BusinessCondition)
            {
                //Below properties are available only for Business rules.

                if (String.Compare(this.RuleDefinition, objectToBeCompared.RuleDefinition) != 0)
                {
                    return false;
                }

                if (this.DisplayType != objectToBeCompared.DisplayType)
                {
                    return false;
                }

                if (!this.DisplayList.Equals(objectToBeCompared.DisplayList))
                {
                    return false;
                }

                if (String.Compare(this.TargetAttributeName, objectToBeCompared.TargetAttributeName) != 0)
                {
                    return false;
                }

                if (this.TargetLocale != objectToBeCompared.TargetLocale)
                {
                    return false;
                }
            }
            else
            {
                if (this.BusinessConditionRules != null && this.BusinessConditionRules.Count > 0 && !this.BusinessConditionRules.Equals(objectToBeCompared.BusinessConditionRules))
                {
                    return false;
                }
            }

            if (this.IsSystemRule != objectToBeCompared.IsSystemRule)
            {
                return false;
            }

            if (this.PublishedVersionId != objectToBeCompared.PublishedVersionId)
            {
                return false;
            }

            if (this.Sequence != objectToBeCompared.Sequence)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether specified object is equal to the current object include Context Mapping (RuleMapIds and RuleMapNames)
        /// </summary>
        /// <param name="obj">MDMRule object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public Boolean EqualsWithContextMapping(MDMRule obj)
        {
            return Equals(obj) && ValueTypeHelper.CollectionEquals(this.RuleMapIds, obj.RuleMapIds);
        }

        /// <summary>
        /// Compares MDMRule object with current MDMRule object
        /// This method will compare object, its attributes and Values.
        /// If current object has more attributes than object to be compared, extra attributes will be ignored.
        /// If attribute to be compared has attributes which is not there in current collection, it will return false.
        /// </summary>
        /// <param name="subSetMDMRule">Indicates MDMRule object to be compared with current MDMRule object</param>
        /// <param name="compareIds">Indicates whether to compare ids for the current object or not</param>
        /// <returns>Returns True : If both are same. False : otherwise</returns>
        public Boolean IsSuperSetOf(MDMRule subSetMDMRule, Boolean compareIds = false)
        {
            if (!base.IsSuperSetOf(subSetMDMRule, compareIds))
            {
                return false;
            }

            if (this.RuleType != subSetMDMRule.RuleType)
            {
                return false;
            }

            if (String.Compare(this.Description, subSetMDMRule.Description) != 0)
            {
                return false;
            }

            if (String.Compare(TargetAttributeName, subSetMDMRule.TargetAttributeName) != 0)
            {
                return false;
            }

            if (TargetLocale != subSetMDMRule.TargetLocale)
            {
                return false;
            }

            if (String.Compare(this.RuleDefinition, subSetMDMRule.RuleDefinition) != 0)
            {
                return false;
            }

            //if (this.RuleContext.Equals(subSetMDMRule.RuleContext))
            //{
            //    return false;
            //}

            if (this.DisplayType != subSetMDMRule.DisplayType)
            {
                return false;
            }

            if (!this.DisplayList.Equals(subSetMDMRule.DisplayList))
            {
                return false;
            }

            if (this.IsSystemRule != subSetMDMRule.IsSystemRule)
            {
                return false;
            }

            if (this.IsEnabled != subSetMDMRule.IsEnabled)
            {
                return false;
            }

            if (this.RuleStatus != subSetMDMRule.RuleStatus)
            {
                return false;
            }

            if (this.PublishedVersionId != subSetMDMRule.PublishedVersionId)
            {
                return false;
            }

            if (this.Sequence != subSetMDMRule.Sequence)
            {
                return false;
            }

            if (this.BusinessConditionRules.Equals(subSetMDMRule.BusinessConditionRules))
            {
                return false;
            }

            return true;
        }

        #endregion Utility Mehtods

        #endregion Public Methods

        #region Overrides Methods

        /// <summary>
        /// Determines whether specified object is equal to the current object
        /// </summary>
        /// <param name="obj">MDMRule object which needs to be compared</param>
        /// <returns>Result of the comparison in Boolean</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is MDMRule)
            {
                MDMRule objectToBeCompared = obj as MDMRule;

                if (!IsEqualsExceptState(objectToBeCompared))
                {
                    return false;
                }

                if (this.IsEnabled != objectToBeCompared.IsEnabled)
                {
                    return false;
                }

                if (this.RuleStatus != objectToBeCompared.RuleStatus)
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

            hashCode = base.GetHashCode() ^ this.ReferenceId.GetHashCode() ^ this.RuleType.GetHashCode() ^ this.DisplayType.GetHashCode() ^ this.IsSystemRule.GetHashCode() ^
                       this.IsEnabled.GetHashCode() ^ this.RuleStatus.GetHashCode() ^ this.PublishedVersionId.GetHashCode() ^ this.Sequence.GetHashCode();

            if (!String.IsNullOrWhiteSpace(this.Description))
            {
                hashCode = hashCode ^ this.Description.GetHashCode();
            }

            if (!String.IsNullOrWhiteSpace(this.RuleDefinition))
            {
                hashCode = hashCode ^ this.RuleDefinition.GetHashCode();
            }

            if (this.RuleContext != null)
            {
                hashCode = hashCode ^ this.RuleContext.GetHashCode();
            }

            if (this.DisplayList != null)
            {
                hashCode = hashCode ^ this.DisplayList.GetHashCode();
            }

            if (this.BusinessConditionRules != null)
            {
                hashCode = hashCode ^ this.BusinessConditionRules.GetHashCode();
            }

            if (!String.IsNullOrWhiteSpace(this.TargetAttributeName))
            {
                hashCode = hashCode ^ this.TargetAttributeName.GetHashCode();
            }

            if (this.TargetLocale != LocaleEnum.UnKnown)
            {
                hashCode = hashCode ^ this.TargetLocale.GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Gets Xml representation of MDMRule Object
        /// </summary>
        /// <returns>MDMRule Object as Xml</returns>
        public override String ToXml()
        {
            #region Sample Xml

            /*  
                BusinessRule:
                ------------
                <MDMRule Id="-1" ReferenceId="-1" Name="ValidateCommonAttributes" RuleType="ValidationRule" Description="" RuleDefinition="" DisplayType="" DisplayList="" 
                         IsSystemRule="false" IsEnabled="true" RuleStatus="Draft" PublishedVersionId="-1" Action="Create">
                </MDMRule>
            */

            /*
                BusinessCondition:
                ------------------
                <MDMRule Id="-1" ReferenceId="-1" Name="ABCBusinessCondition" RuleType="BusinessCondition" Description="" RuleDefinition="" DisplayType="" DisplayList="" 
                         IsSystemRule="false" IsEnabled="true" RuleStatus="Draft" PublishedVersionId="-1" Action="Create">
                    <BusinessConditionRules>
                        <KeyValuePairItems>
                            <KeyValuePairItem Key="-1" Value="ValidateCommonAttributes">
                        </KeyValuePairItems>
                    </BusinessConditionRules>
                </MDMRule>
            */

            #endregion Sample Xml

            String outputXml = String.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
                {
                    //MDMRule node start
                    xmlWriter.WriteStartElement("MDMRule");

                    xmlWriter.WriteAttributeString("Id", Id.ToString());
                    xmlWriter.WriteAttributeString("ReferenceId", ReferenceId.ToString());
                    xmlWriter.WriteAttributeString("Name", Name);
                    xmlWriter.WriteAttributeString("RuleType", RuleType.ToString());
                    xmlWriter.WriteAttributeString("Description", Description);
                    xmlWriter.WriteAttributeString("RuleDefinition", RuleDefinition);
                    xmlWriter.WriteAttributeString("DisplayType", DisplayType.ToString());
                    xmlWriter.WriteAttributeString("IsSystemRule", IsSystemRule.ToString());
                    xmlWriter.WriteAttributeString("IsEnabled", IsEnabled.ToString());
                    xmlWriter.WriteAttributeString("RuleStatus", RuleStatus.ToString());
                    xmlWriter.WriteAttributeString("PublishedVersionId", PublishedVersionId.ToString());
                    xmlWriter.WriteAttributeString("TargetAttributeName", TargetAttributeName);
                    xmlWriter.WriteAttributeString("TargetLocale", TargetLocale.ToString());
                    xmlWriter.WriteAttributeString("Sequence", Sequence.ToString());
                    xmlWriter.WriteAttributeString("Action", Action.ToString());

                    if (this.RuleContext != null)
                    {
                        xmlWriter.WriteRaw(this.RuleContext.ToXml());
                    }

                    if (this.DisplayList != null)
                    {
                        xmlWriter.WriteRaw(this.DisplayList.ToXml());
                    }

                    xmlWriter.WriteStartElement("BusinessConditionRules");

                    if (this.BusinessConditionRules != null)
                    {
                        xmlWriter.WriteRaw(this.BusinessConditionRules.ToXml());
                    }

                    xmlWriter.WriteEndElement();

                    //MDMRule node end
                    xmlWriter.WriteEndElement();
                }

                //Get the output XML
                outputXml = sw.ToString();
            }

            return outputXml;
        }

        #endregion Overrides Methods

        #region IClonable Members

        /// <summary>
        /// Gets a cloned instance of the current MDMRule object
        /// </summary>
        /// <returns>Cloned instance of the current MDMRule object</returns>
        Object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion IClonable Members

        #region Private Methods

        /// <summary>
        /// Loads MDMRule from XML
        /// </summary>
        /// <param name="valuesAsXml">Indicates xml representation of MDMRule</param>
        private void LoadMDMRuleFromXml(String valuesAsXml)
        {
            #region Sample Xml

            /*  
                BusinessRule:
                ------------
                <MDMRule Id="-1" ReferenceId="-1" Name="ValidateCommonAttributes" RuleType="ValidationRule" Description="" RuleDefinition="" DisplayType="" DisplayList="" 
                         IsSystemRule="false" IsEnabled="true" RuleStatus="Draft" PublishedVersionId="-1" Action="Create">
                </MDMRule>
            */

            /*
                BusinessCondition:
                ------------------
                <MDMRule Id="-1" ReferenceId="-1" Name="ABCBusinessCondition" RuleType="BusinessCondition" Description="" RuleDefinition="" DisplayType="" DisplayList="" 
                         IsSystemRule="false" IsEnabled="true" RuleStatus="Draft" PublishedVersionId="-1" Action="Create">
                    <BusinessConditionRules>
                        <KeyValuePairItems>
                            <KeyValuePairItem Key="-1" Value="ValidateCommonAttributes">
                        </KeyValuePairItems>
                    </BusinessConditionRules>
                </MDMRule>
            */

            #endregion Sample Xml

            if (!String.IsNullOrWhiteSpace(valuesAsXml))
            {
                using (XmlTextReader reader = new XmlTextReader(valuesAsXml, XmlNodeType.Element, null))
                {
                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRule")
                        {
                            #region Read MDMRule Attributes

                            if (reader.HasAttributes)
                            {
                                if (reader.MoveToAttribute("Id"))
                                {
                                    this.Id = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Id);
                                }

                                if (reader.MoveToAttribute("ReferenceId"))
                                {
                                    this.ReferenceId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this._referenceId);
                                }

                                if (reader.MoveToAttribute("Name"))
                                {
                                    this.Name = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RuleType"))
                                {
                                    ValueTypeHelper.EnumTryParse<MDMRuleType>(reader.ReadContentAsString(), true, out this._ruleType);
                                }

                                if (reader.MoveToAttribute("Description"))
                                {
                                    this.Description = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("RuleDefinition"))
                                {
                                    this.RuleDefinition = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("DisplayType"))
                                {
                                    ValueTypeHelper.EnumTryParse<DisplayType>(reader.ReadContentAsString(), true, out this._displayType);
                                }

                                if (reader.MoveToAttribute("IsSystemRule"))
                                {
                                    this.IsSystemRule = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsSystemRule);
                                }

                                if (reader.MoveToAttribute("IsEnabled"))
                                {
                                    this.IsEnabled = ValueTypeHelper.BooleanTryParse(reader.ReadContentAsString(), this.IsEnabled);
                                }

                                if (reader.MoveToAttribute("RuleStatus"))
                                {
                                    ValueTypeHelper.EnumTryParse<RuleStatus>(reader.ReadContentAsString(), true, out this._ruleStatus);
                                }

                                if (reader.MoveToAttribute("PublishedVersionId"))
                                {
                                    this.PublishedVersionId = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.PublishedVersionId);
                                }

                                if (reader.MoveToAttribute("TargetAttributeName"))
                                {
                                    this.TargetAttributeName = reader.ReadContentAsString();
                                }

                                if (reader.MoveToAttribute("TargetLocale"))
                                {
                                    ValueTypeHelper.EnumTryParse<LocaleEnum>(reader.ReadContentAsString(), true, out this._targetLocale);
                                }

                                if (reader.MoveToAttribute("Action"))
                                {
                                    this.Action = ValueTypeHelper.GetAction(reader.ReadContentAsString());
                                }

                                if (reader.MoveToAttribute("Sequence"))
                                {
                                    this.Sequence = ValueTypeHelper.Int32TryParse(reader.ReadContentAsString(), this.Sequence);
                                }

                                reader.Read();
                            }

                            #endregion
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "MDMRuleContext")
                        {
                            String mdmRuleContextAsXml = reader.ReadOuterXml();

                            if (mdmRuleContextAsXml != null)
                            {
                                this.RuleContext = new MDMRuleContext(mdmRuleContextAsXml);
                            }

                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "DisplayList")
                        {
                            String mdmObjectsAsXml = reader.ReadOuterXml();

                            if (mdmObjectsAsXml != null)
                            {
                                this.DisplayList = new MDMRuleDisplayList(mdmObjectsAsXml);
                            }

                            reader.Read();
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "BusinessConditionRules")
                        {
                            String bcRulesAsXml = reader.ReadOuterXml();

                            if (bcRulesAsXml != null)
                            {
                                this.BusinessConditionRules = new MDMRuleCollection(bcRulesAsXml);
                            }

                            reader.Read();
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

        #endregion Private Methods

        #endregion Methods
    }
}
