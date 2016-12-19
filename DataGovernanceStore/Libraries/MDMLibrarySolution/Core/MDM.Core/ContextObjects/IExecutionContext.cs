using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods used for setting the execution context including the current execution information.
    /// </summary>
    public interface IExecutionContext
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Gets the current Rule execution information.
        /// </summary>
        /// <returns>Returns IRuleExecutionInfo</returns>
        IRuleExecutionInfo GetRuleExecutionInfo();

        /// <summary>
        /// Gets the IExecutionContext in xml format
        /// </summary>
        /// <returns>Returns IExecutionContext in xml format</returns>
        String ToXml();

        /// <summary>
        /// Gets the SecurityContext
        /// </summary>
        /// <returns>Returns ISecurityContext</returns>
        ISecurityContext GetSecurityContext();

        /// <summary>
        /// Gets the CallerContext
        /// </summary>
        /// <returns>Returns ICallerContext</returns>
        ICallerContext GetCallerContext();

        /// <summary>
        /// Gets the DataModel Context
        /// </summary>
        /// <returns>Returns ICallerContext</returns>
        ICallDataContext GetCallDataContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentExecutionContext"></param>
        void Merge(ExecutionContext parentExecutionContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        Boolean Compare(ExecutionContext executionContext);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ExecutionContext Clone();

        #endregion
    }
}