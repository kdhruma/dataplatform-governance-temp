using System;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Type of a BusinessRuleSetRule
    /// </summary>
    [DataContract]
    public class BusinessRuleSetRule : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting BusinessRule_RuleSet Id
        /// </summary>
        private Int32 _businessRuleSetId = -1;

        /// <summary>
        /// Field denoting ShortName of BusinessRule_RuleSet
        /// </summary>
        private String _businessRuleSetName = String.Empty;

        /// <summary>
        /// Field denoting BusinessRule_Rule Id
        /// </summary>
        private Int32 _businessRuleId = -1;

        /// <summary>
        /// Field denoting ShortName of BusinessRule_Rule name
        /// </summary>
        private String _businessRuleName = String.Empty;

        /// <summary>
        /// Field denoting Application_ContextDefinition Id
        /// </summary>
        private Int32 _applicationContextDefinitionId = -1;

        /// <summary>
        /// Field denoting Org Id
        /// </summary>
        private Int32 _orgId = 0;

        /// <summary>
        /// Field denoting ShortName of Org
        /// </summary>
        private String _orgName = String.Empty;

        /// <summary>
        /// Field denoting Catalog Id
        /// </summary>
        private Int32 _containerId = 0;

        /// <summary>
        /// Field denoting ShortName of Catalog
        /// </summary>
        private String _containerName = String.Empty;

        /// <summary>
        /// Field denoting Category Id
        /// </summary>
        private Int32 _categoryId = 0;

        /// <summary>
        /// Field denoting ShortName of Category
        /// </summary>
        private String _categoryName = String.Empty;

        /// <summary>
        /// Field denoting CNode Id
        /// </summary>
        private Int64 _entityId = 0;

        /// <summary>
        /// Field denoting ShortName of CNode name
        /// </summary>
        private String _entityName = String.Empty;

        /// <summary>
        /// Field denoting Attribute Id
        /// </summary>
        private Int32 _attributeId = 0;

        /// <summary>
        /// Field denoting ShortName of Attribute
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field denoting NodeType Id
        /// </summary>
        private Int32 _entityTypeId = 0;

        /// <summary>
        /// Field denoting ShortName of NodeType
        /// </summary>
        private String _entityTypeName = String.Empty;

        /// <summary>
        /// Field denoting RelationshipType Id
        /// </summary>
        private Int32 _relationTypeId = 0;

        /// <summary>
        /// Field denoting ShortName of RelationshipType
        /// </summary>
        private String _relationTypeName = String.Empty;

        /// <summary>
        /// Field denoting Security_user Id
        /// </summary>
        private Int32 _securityUserId = 0;

        /// <summary>
        /// Field denoting Login of Security_user
        /// </summary>
        private String _securityUserLoginName = String.Empty;

        /// <summary>
        /// Field denoting Security_Role Id
        /// </summary>
        private Int32 _securityRoleId = 0;

        /// <summary>
        /// Field denoting ShortName of Security_Role
        /// </summary>
        private String _securityRoleName = String.Empty;

        /// <summary>
        /// Field denoting Seq of BusinessRuleSetRule
        /// </summary>
        private Int32 _sequence = -1;

        /// <summary>
        /// Field denoting Priority of BusinessRuleSetRule
        /// </summary>
        private Int32 _priority = -1;

        /// <summary>
        /// Field denoting ActiveFlag for BusinessRuleSetRule
        /// </summary>
        private Boolean _activeFlag = true;

        /// <summary>
        /// Field denoting ViewContext Id of BusinessRuleSetRule
        /// </summary>
        private Int32 _businessRuleViewContextId = 0;

        /// <summary>
        /// Field denoting ViewContext name of BusinessRuleSetRule
        /// </summary>
        private String _businessRuleViewContextName = String.Empty;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public BusinessRuleSetRule()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleSetRule</param>
        public BusinessRuleSetRule(Int32 id)
            : base(id)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting the BusinessRuleSetI d of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleSetId
        {
            get { return this._businessRuleSetId; }
            set { this._businessRuleSetId = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRuleSet Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String BusinessRuleSetName
        {
            get { return this._businessRuleSetName; }
            set { this._businessRuleSetName = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRule Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleId
        {
            get { return this._businessRuleId; }
            set { this._businessRuleId = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRule Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String BusinessRuleName
        {
            get { return this._businessRuleName; }
            set { this._businessRuleName = value; }
        }

        /// <summary>
        /// Property denoting the ApplicationContextDefinition Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 ApplicationContextDefinitionId
        {
            get { return this._applicationContextDefinitionId; }
            set { this._applicationContextDefinitionId = value; }
        }

        /// <summary>
        /// Property denoting the Org Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 OrgId
        {
            get { return this._orgId; }
            set { this._orgId = value; }
        }

        /// <summary>
        /// Property denoting the Org Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String OrgName
        {
            get { return this._orgName; }
            set { this._orgName = value; }
        }

        /// <summary>
        /// Property denoting the Container Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 ContainerId
        {
            get { return this._containerId; }
            set { this._containerId = value; }
        }

        /// <summary>
        /// Property denoting the Container Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String ContainerName
        {
            get { return this._containerName; }
            set { this._containerName = value; }
        }

        /// <summary>
        /// Property denoting the Category Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 CategoryId
        {
            get { return this._categoryId; }
            set { this._categoryId= value; }
        }

        /// <summary>
        /// Property denoting the Category Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String CategoryName
        {
            get { return this._categoryName; }
            set { this._categoryName = value; }
        }

        /// <summary>
        /// Property denoting the Entity Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int64 EntityId
        {
            get { return this._entityId; }
            set { this._entityId = value; }
        }

        /// <summary>
        /// Property denoting the Entity Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String EntityName
        {
            get { return this._entityName; }
            set { this._entityName = value; }
        }

        /// <summary>
        /// Property denoting the Attribute Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 AttributeId
        {
            get { return this._attributeId; }
            set { this._attributeId = value; }
        }

        /// <summary>
        /// Property denoting the Attribute Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return this._attributeName; }
            set { this._attributeName = value; }
        }

        /// <summary>
        /// Property denoting the EntityType Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 EntityTypeId
        {
            get { return this._entityTypeId; }
            set { this._entityTypeId = value; }
        }

        /// <summary>
        /// Property denoting the EntityType Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String EntityTypeName
        {
            get { return this._entityTypeName; }
            set { this._entityTypeName = value; }
        }

        /// <summary>
        /// Property denoting the RelationType Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 RelationTypeId
        {
            get { return this._relationTypeId; }
            set { this._relationTypeId = value; }
        }

        /// <summary>
        /// Property denoting the RelationType Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String RelationTypeName
        {
            get { return this._relationTypeName; }
            set { this._relationTypeName = value; }
        }

        /// <summary>
        /// Property denoting the SecurityUser Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 SecurityUserId
        {
            get { return this._securityUserId; }
            set { this._securityUserId = value; }
        }

        /// <summary>
        /// Property denoting the SecurityRole Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 SecurityRoleId
        {
            get { return this._securityRoleId; }
            set { this._securityRoleId = value; }
        }

        /// <summary>
        /// Property denoting the SecurityRole Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String SecurityRoleName
        {
            get { return this._securityRoleName; }
            set { this._securityRoleName = value; }
        }

        /// <summary>
        /// Property denoting the Sequence of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 Sequence
        {
            get { return this._sequence; }
            set { this._sequence = value; }
        }

        /// <summary>
        /// Property denoting the Priority of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 Priority
        {
            get { return this._priority; }
            set { this._priority = value; }
        }

        /// <summary>
        /// Property denoting the ActiveFlag of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Boolean ActiveFlag
        {
            get { return this._activeFlag; }
            set { this._activeFlag = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRuleViewContext Id of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public Int32 BusinessRuleViewContextId
        {
            get { return this._businessRuleViewContextId; }
            set { this._businessRuleViewContextId = value; }
        }

        /// <summary>
        /// Property denoting the BusinessRuleViewContext Name of BusinessRuleSetRule
        /// </summary>
        [DataMember]
        public String BusinessRuleViewContextName
        {
            get { return this._businessRuleViewContextName; }
            set { this._businessRuleViewContextName = value; }
        }


        #endregion
    }
}
