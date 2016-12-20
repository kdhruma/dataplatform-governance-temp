using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing relationship type entity type mapping related information.
    /// </summary>
    public interface IRelationshipTypeEntityTypeMapping : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting mapped RelationshipType Id
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting the mapped RelationshipType Name
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Property denoting the mapped RelationshipType long name
        /// </summary>
        String RelationshipTypeLongName { get; set; }

        /// <summary>
        /// Property denoting mapped Entity Type Id
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the mapped Entity Type Name
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting the mapped Entity Type long name
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting the DrillDown for this Mapping
        /// </summary>
        Boolean DrillDown { get; set; }

        /// <summary>
        /// Property denoting the IsDefaultRelation for this Mapping
        /// </summary>
        Boolean IsDefaultRelation { get; set; }

        /// <summary>
        /// Property denoting the excludable for this Mapping
        /// </summary>
        Boolean Excludable { get; set; }

        /// <summary>
        /// Indicates the Read Only for this Mapping
        /// </summary>
        Boolean ReadOnly { get; set; }

        /// <summary>
        /// Property denoting the ValidationRequired for this Mapping
        /// </summary>
        Boolean ValidationRequired { get; set; }

        /// <summary>
        /// Property denoting the ShowValidFlagInGrid for this Mapping
        /// </summary>
        Boolean ShowValidFlagInGrid { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeEntityType object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXML();

        /// <summary>
        /// Clone RelationshipTypeEntityTypeMapping object
        /// </summary>
        /// <returns>cloned copy of IRelationshipTypeEntityTypeMapping object.</returns>
        IRelationshipTypeEntityTypeMapping Clone();

        /// <summary>
        /// Delta Merge of RelationshipTypeEntityTypeMapping
        /// </summary>
        /// <param name="deltaRelationshipTypeEntityTypeMapping">RelationshipTypeEntityTypeMapping that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipTypeEntityTypeMapping instance</returns>
        IRelationshipTypeEntityTypeMapping MergeDelta(IRelationshipTypeEntityTypeMapping deltaRelationshipTypeEntityTypeMapping, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}