using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity type attribute mapping.
    /// </summary>
    public interface IEntityTypeAttributeMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting EntityType Id.
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting Entity Type Short Name.
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting Entity Type Long Name.
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting the Attribute Id.
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting the Attribute Short Name.
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Property denoting the Attribute Long Name.
        /// </summary>
        String AttributeLongName { get; set; }

        /// <summary>
        /// Property denoting the Attribute Parent Id.
        /// </summary>
        Int32 AttributeParentId { get; set; }

        /// <summary>
        /// Property denoting Attribute Parent Short Name.
        /// </summary>
        String AttributeParentName { get; set; }

        /// <summary>
        /// Property denoting Attribute Parent Long Name.
        /// </summary>
        String AttributeParentLongName { get; set; }

        /// <summary>
        /// Property denoting Extension.
        /// </summary>
        String Extension { get; set; }

        /// <summary>
        /// Property denoting ReadOnly.
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property denoting Required.
        /// </summary>
        Boolean Required { get; set; }

        /// <summary>
        /// Property denoting Show At Creation.
        /// </summary>
        Boolean ShowAtCreation { get; set; }

        /// <summary>
        /// Property denoting the Sort Order.
        /// </summary>
        Int32 SortOrder { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of EntityTypeAttributeMapping object
        /// </summary>
        /// <returns>XML representation of EntityTypeAttributeMapping object</returns>
        String ToXML();

        #endregion

        /// <summary>
        /// Clone EntityTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IEntityTypeAttributeMapping object.</returns>
        IEntityTypeAttributeMapping Clone();

        /// <summary>
        /// Delta Merge of EntityTypeAttributeMapping
        /// </summary>
        /// <param name="deltaEntityTypeAttributeMapping">EntityTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IEntityTypeAttributeMapping instance</returns>
        IEntityTypeAttributeMapping MergeDelta(IEntityTypeAttributeMapping deltaEntityTypeAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);


        #endregion
    }
}