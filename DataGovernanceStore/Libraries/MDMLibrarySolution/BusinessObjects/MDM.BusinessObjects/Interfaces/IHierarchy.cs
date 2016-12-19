using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get hierarchy related information.
    /// </summary>
    public interface IHierarchy : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property indicates if entities can be added only for a leaf category
        /// </summary>
        Boolean LeafNodeOnly { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of hierarchy
        /// </summary>
        /// <returns>Xml representation of hierarchy</returns>
        String ToXml();

        /// <summary>
        /// Clone hierarchy object
        /// </summary>
        /// <returns>Cloned copy of hierarchy object.</returns>
        IHierarchy Clone();

        /// <summary>
        /// Gets the attributes belonging to the Hierarchy.
        /// </summary>
        /// <returns>Attribute Collection Interface</returns>
        IAttributeCollection GetAttributes();

        /// <summary>
        /// Sets the attributes belonging to the Hierarchy.
        /// </summary>
        /// <param name="iAttributes">Collection of attributes to be set.</param>
        void SetAttributes(IAttributeCollection iAttributes);

        /// <summary>
        /// Delta Merge of hierarchy
        /// </summary>
        /// <param name="deltaHierarchy">Hierarchy that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged hierarchy instance</returns>
        IHierarchy MergeDelta(IHierarchy deltaHierarchy, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}