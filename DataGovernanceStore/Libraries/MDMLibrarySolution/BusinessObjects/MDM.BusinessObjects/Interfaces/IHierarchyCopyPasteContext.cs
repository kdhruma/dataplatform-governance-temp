using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the hierarchy context.
    /// </summary>
    public interface IHierarchyCopyPasteContext
    {
        #region Properties

        /// <summary>
        /// Property denoting source hierarchy Id. From which we want to copy categories
        /// </summary>
        Int32 SourceHierarchyId { get; set; }

        /// <summary>
        /// Property denoting target hierarchy Id. To which we want to paste categories
        /// </summary>
        Int32 TargetHierarchyId { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents HierarchyCopyPasteContext in Xml format
        /// </summary>
        /// <returns>String representation of current HierarchyCopyPasteContext object</returns>
        String ToXml();

        /// <summary>
        /// Create clone of HierarchyCopyPasteContext
        /// </summary>
        /// <returns>Clone of HierarchyCopyPasteContext</returns>
        IHierarchyCopyPasteContext Clone();

        #endregion Methods
    }
}
