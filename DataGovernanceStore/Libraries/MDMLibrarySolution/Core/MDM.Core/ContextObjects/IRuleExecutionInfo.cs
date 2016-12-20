using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Represents the interface that contains current execution MDMRule information
    /// </summary>
    public interface IRuleExecutionInfo
    {
        #region Properties

        /// <summary>
        /// Property denotes the MDMRule Id
        /// </summary>
        Int32 RuleId
        {
            get;
        }

        /// <summary>
        /// Property denotes the Business condition Rule Id
        /// </summary>
        Int32 BusinessConditionRuleId
        {
            get;
        }

        /// <summary>
        /// Property denotes the Rule Map Id
        /// </summary>
        Int32 RuleMapId
        {
            get;
        }

        /// <summary>
        /// Property denotes the name of the MDMRule
        /// </summary>
        String RuleName
        {
            get;
        }

        /// <summary>
        /// Property denotes the name of the Business condition
        /// </summary>
        String BusinessConditionRuleName
        {
            get;
        }

        /// <summary>
        /// Property denotes the name of the MDMRule map
        /// </summary>
        String MDMRuleMapName
        {
            get;
        }

        /// <summary>
        /// Checks whether the MDMRule is executing due to Business Conditon or not.
        /// </summary>
        Boolean IsBusinessConditionExecution
        {
            get;
        }

        #endregion Properties
    }
}
