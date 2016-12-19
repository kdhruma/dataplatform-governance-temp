using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity type.
    /// </summary>
    public interface IEntityType : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting the catalog branch level of entity type
        /// </summary>
        Int32 CatalogBranchLevel { get; set; }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>mo
        String ObjectType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Type
        /// </summary>
        /// <returns>Xml representation of Entity Type</returns>
        String ToXML();

        /// <summary>
        /// Clone EntityType object
        /// </summary>
        /// <returns>cloned copy of EntityType object.</returns>
        IEntityType Clone();

        /// <summary>
        /// Delta Merge of entity type
        /// </summary>
        /// <param name="deltaEntityType">EntityType that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged entity type instance</returns>
        IEntityType MergeDelta(IEntityType deltaEntityType, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}
