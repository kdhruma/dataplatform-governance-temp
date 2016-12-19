using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using ProtoBuf;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents the class that contain BaseMDMRule context filter properties and methods
    /// </summary>
    [DataContract]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [KnownType(typeof(ApplicationContextCollection))]
    public class BaseMDMRuleContextFilter
    {
        #region Fields

        /// <summary>
        /// Indicates the list of entity Ids
        /// </summary>
        private Collection<Int64> _entityIds = null;

        /// <summary>
        /// Indicates the list of MDMRule Ids
        /// </summary>
        private Collection<Int32> _ruleIds = null;

        /// <summary>
        /// Indicates the application contexts
        /// </summary>
        private ApplicationContextCollection _applicationContexts = null;

        /// <summary>
        /// Indicates the MDMRule type
        /// </summary>
        private MDMRuleType _ruleType = MDMRuleType.UnKnown;

        /// <summary>
        /// Whether to load Business condition's validation rules or not.
        /// This applicable if Business condition rules are available.
        /// </summary>
        private Boolean _loadValidationRules = false;

        /// <summary>
        /// Indicates the Rule status
        /// </summary>
        private RuleStatus _ruleStatus = RuleStatus.Unknown;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property denotes the Entity Id list
        /// </summary>
        [DataMember]
        public Collection<Int64> EntityIds
        {
            get
            {
                if (this._entityIds == null)
                {
                    this._entityIds = new Collection<Int64>();
                }

                return _entityIds;
            }
            set
            {
                _entityIds = value;
            }
        }

        /// <summary>
        /// Property denotes the MdMRule Id list
        /// </summary>
        [DataMember]
        public Collection<Int32> MDMRuleIds
        {
            get
            {
                if (this._ruleIds == null)
                {
                    this._ruleIds = new Collection<Int32>();
                }

                return _ruleIds;
            }
            set
            {
                _ruleIds = value;
            }
        }

        /// <summary>
        /// Property denotes the Application Contexts
        /// </summary>
        [DataMember]
        public ApplicationContextCollection ApplicationContexts
        {
            get
            {
                if (_applicationContexts == null)
                {
                    _applicationContexts = new ApplicationContextCollection();
                }

                return _applicationContexts;
            }
            set
            {
                _applicationContexts = value;
            }
        }

        /// <summary>
        /// Property denotes the MDMRuleType
        /// </summary>
        [DataMember]
        public MDMRuleType MDMRuleType
        {
            get
            {
                return _ruleType;
            }
            set
            {
                _ruleType = value;
            }
        }

        /// <summary>
        /// Whether to load Business condition's validation rules or not.
        /// This applicable if Business condition rules are available.
        /// </summary>
        [DataMember]
        public Boolean LoadValidationRules
        {
            get
            {
                return _loadValidationRules;
            }
            set
            {
                _loadValidationRules = value;
            }
        }

        /// <summary>
        /// Property denotes the Rule status
        /// </summary>
        [DataMember]
        public RuleStatus RuleStatus
        {
            get
            {
                return _ruleStatus;
            }
            set
            {
                _ruleStatus = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Set the application context collection for the context filter
        /// </summary>
        /// <param name="applicationContexts">Indicates the application context collection object</param>
        public void SetApplicationContexts(IApplicationContextCollection applicationContexts)
        {
            this._applicationContexts = (ApplicationContextCollection)applicationContexts;
        }

        #endregion Methods
    }
}
