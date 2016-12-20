using System;

namespace MDM.Core
{
    /// <summary>
    /// Exposes methods or properties to set or get the activity base related information.
    /// </summary>
    public interface IActivityBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        Guid ActivityId { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid ParentActivityId { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid OperationId { get; }

        /// <summary>
        ///
        /// </summary>
        String ActivityName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of IActivityBase
        /// </summary>
        /// <returns>Xml representation of IActivityBase</returns>
        String ToXml();

        #endregion
    }
}
