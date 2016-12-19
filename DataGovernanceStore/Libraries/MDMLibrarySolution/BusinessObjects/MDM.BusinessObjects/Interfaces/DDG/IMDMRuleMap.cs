using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Represents the interface that contains MDMRule Mapping information
    /// </summary>
    public interface IMDMRuleMap : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the event id
        /// </summary>
        Int32 EventId { get; set; }

        /// <summary>
        /// Indicates the name of the event
        /// </summary>
        String EventName { get; set; }

        /// <summary>
        /// Indicates whether rule should be executed async or not
        /// </summary>
        Boolean IsAsyncRule { get; set; }

        /// <summary>
        /// Indicates whether rule mapped under the current context and event is enabled or not
        /// </summary>
        Boolean IsEnabled { get; set; }

        /// <summary>
        /// Property denotes the MDMEvent Type
        /// </summary>
        MDMEventType EventType { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the application context of MDMRule map
        /// </summary>
        /// <returns>ApplicationContext</returns>
        IApplicationContext GetApplicationContext();

        /// <summary>
        /// Set the application context to MDMRule map
        /// </summary>
        /// <param name="applicationContext">Indicates the application context</param>
        void SetApplicationContext(IApplicationContext applicationContext);

        /// <summary>
        /// Get the WorkflowInfo in MDMRule Map
        /// </summary>
        /// <returns>WorkflowInfo</returns>
        IWorkflowInfo GetWorkflowInfo();

        /// <summary>
        /// Set the WorkflowInfo in MDMRule Map
        /// </summary>
        /// <param name="workflowInfo">Indicates the WorkflowInfo</param>
        void SetWorkflowInfo(IWorkflowInfo workflowInfo);

        /// <summary>
        /// Get Last Modified AuditInfo
        /// </summary>
        /// <returns>AuditInfo</returns>
        IAuditInfo GetLastModifiedAuditInfo();

         /// <summary>
        /// Returns Rules and BusinessConditions mapped to Mapping Context
        /// </summary>
        /// <returns></returns>
        IMDMRuleMapRuleCollection GetMDMRuleMapRules();

        #endregion Methods
    }
}