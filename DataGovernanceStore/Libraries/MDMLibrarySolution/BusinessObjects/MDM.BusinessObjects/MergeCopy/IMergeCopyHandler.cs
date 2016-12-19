using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.BusinessObjects.MergeCopy
{
    using MDM.Interfaces;
    /// <summary>
    /// Handler interface for MergeCopy
    /// </summary>
    public interface IMergeCopyHandler
    {

        #region
        /// <summary>
        /// This method is called by the engine optionally on a per entity, attribute, relationship, relationship attribute 
        /// </summary>
        /// <param name="conflictContext"></param>
        /// <param name="callerContext"></param>
        /// <param name="entityOperationResult"></param>
        /// <returns></returns>

        bool MergeUsingMergeContext( MergeCopyContext.MergeCopyConflictContext conflictContext, MergeCopyContext.MergeCopyCallerContext callerContext, IEntityOperationResult entityOperationResult);

        #endregion

   
    }
}
