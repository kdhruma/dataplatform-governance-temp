using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for mapping relationship type with attribute.
    /// </summary>
    public interface IRelationshipTypeAttributeMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting Relationship Type Id.
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting Relationship Type Short Name.
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Property denoting Relationship Type Long Name.
        /// </summary>
        String RelationshipTypeLongName { get; set; }

        /// <summary>
        /// Property denoting Attribute Id.
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting Attribute Short Name.
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Property denoting Attribute Long Name.
        /// </summary>
        String AttributeLongName { get; set; }

        /// <summary>
        /// Property denoting Attribute Parent Id.
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
        /// Property denoting Read Only.
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property denoting required.
        /// </summary>
        Boolean Required { get; set; }

        /// <summary>
        /// Property denoting Show At Creation.
        /// </summary>
        Boolean ShowAtCreation { get; set; }

        /// <summary>
        /// Property denoting Show Inline.
        /// </summary>
        Boolean ShowInline { get; set; }

        /// <summary>
        /// Property denoting Sort Order.
        /// </summary>
        Int32 SortOrder { get; set; }

        #endregion

        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get XML representation of RelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>XML representation of RelationshipTypeAttributeMapping object</returns>
        String ToXML();

        #endregion

        /// <summary>
        /// Clone RelationshipTypeAttributeMapping object
        /// </summary>
        /// <returns>cloned copy of IRelationshipTypeAttributeMapping object.</returns>
        IRelationshipTypeAttributeMapping Clone();

        /// <summary>
        /// Delta Merge of RelationshipTypeAttributeMapping
        /// </summary>
        /// <param name="deltaRelationshipTypeAttributeMapping">RelationshipTypeAttributeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipTypeAttributeMapping instance</returns>
        IRelationshipTypeAttributeMapping MergeDelta(IRelationshipTypeAttributeMapping deltaRelationshipTypeAttributeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}