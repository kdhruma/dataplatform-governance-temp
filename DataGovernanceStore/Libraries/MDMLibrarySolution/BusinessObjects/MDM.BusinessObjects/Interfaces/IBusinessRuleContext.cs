using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods used for setting the context on which business rules are searched.
    /// </summary>
    /// <typeparam name="TContext">Context</typeparam>
    public interface IBusinessRuleContext<TContext>
    {
        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Executes the business rule on the given context.
        /// </summary>
        /// <param name="context">The data to be processed</param>
        /// <param name="operationResult">Used to report messages as a result of the rule processing</param>
        void Process(TContext context, IOperationResult operationResult);

        #endregion
    }
}
