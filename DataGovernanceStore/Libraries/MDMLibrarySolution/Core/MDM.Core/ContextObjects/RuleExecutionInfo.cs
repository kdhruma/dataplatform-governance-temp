using System;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contains current execution MDMRule information
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class RuleExecutionInfo : ObjectBase, IRuleExecutionInfo
    {
        #region Fields

        /// <summary>
        /// Field denotes the MDMRule id
        /// </summary>
        private Int32 _ruleId;

        /// <summary>
        /// Field denotes the Business condition rule id
        /// </summary>
        private Int32 _bcRuleId;

        /// <summary>
        /// Field denotes the MDMRuleMap id
        /// </summary>
        private Int32 _ruleMapId;

        /// <summary>
        /// Field denotes the Rule name
        /// </summary>
        private String _ruleName;

        /// <summary>
        /// Field denotes the Business condition name
        /// </summary>
        private String _bcRuleName;

        /// <summary>
        /// Field denotes the MDMRule map name
        /// </summary>
        private String _ruleMapName;

        /// <summary>
        /// Field denotes the MdmRule execution due to Business Conditon or not. 
        /// </summary>
        private Boolean _isBusinessConditionExecution = false;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Parameter less constructor
        /// </summary>
        public RuleExecutionInfo()
            : base()
        { 
        
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Property denotes the MDMRuleid
        /// </summary>
        [DataMember]
        public Int32 RuleId
        {
            get
            {
                return _ruleId;
            }
            set
            {
                _ruleId = value;
            }
        }

        /// <summary>
        /// Property denotes the Business condition Rule Id
        /// </summary>
        [DataMember]
        public Int32 BusinessConditionRuleId
        {
            get
            {
                return _bcRuleId;
            }
            set
            {
                _bcRuleId = value;

                if (value > 0)
                {
                    _isBusinessConditionExecution = true;
                }
            }
        }

        /// <summary>
        /// Property denotes the Rule Map Id
        /// </summary>
        [DataMember]
        public Int32 RuleMapId
        {
            get
            {
                return _ruleMapId;
            }
            set
            {
                _ruleMapId = value;
            }
        }

        /// <summary>
        /// Property denotes the name of the MDMRule
        /// </summary>
        [DataMember]
        public String RuleName
        {
            get
            {
                return _ruleName;
            }
            set
            {
                _ruleName = value;
            }
        }

        /// <summary>
        /// Property denotes the name of the Business condition
        /// </summary>
        [DataMember]
        public String BusinessConditionRuleName
        {
            get
            {
                return _bcRuleName;
            }
            set
            {
                _bcRuleName = value;

                if (String.IsNullOrWhiteSpace(value) == false)
                {
                    _isBusinessConditionExecution = true;
                }
            }
        }

        /// <summary>
        /// Property denotes the name of the MDMRule map
        /// </summary>
        [DataMember]
        public String MDMRuleMapName
        {
            get
            {
                return _ruleMapName;
            }
            set
            {
                _ruleMapName = value;
            }
        }

        /// <summary>
        /// Checks whether the MDMrule is executing due to Business Conditon or not.
        /// </summary>
        [DataMember]
        public Boolean IsBusinessConditionExecution
        {
            get
            {
                return _isBusinessConditionExecution;
            }
            set
            {
                _isBusinessConditionExecution = value;
            }
        }

        #endregion Properties
    }
}
