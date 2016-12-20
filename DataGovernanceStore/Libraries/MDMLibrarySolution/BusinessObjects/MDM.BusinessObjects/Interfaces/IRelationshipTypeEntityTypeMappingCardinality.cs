using MDM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Specifies interface providing RelationshipType EntityType Cardinality mapping related information
    /// </summary>
    public interface IRelationshipTypeEntityTypeMappingCardinality : IRelationshipTypeEntityTypeMapping
    {
        #region Properties

        /// <summary>
        /// Property denoting To Entity Type Id
        /// </summary>
        Int32 ToEntityTypeId { get; set; }

        /// <summary>
        /// Property denoting the To Entity Type Name
        /// </summary>
        String ToEntityTypeName { get; set; }

        /// <summary>
        /// Property denoting the To Entity Type Long Name
        /// </summary>
        String ToEntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting the Min Relationships
        /// </summary>
        Int32 MinRelationships { get; set; }

        /// <summary>
        /// Property denoting the Max Relationships
        /// </summary>
        Int32 MaxRelationships { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of RelationshipTypeEntityType object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        new String ToXML();

        /// <summary>
        /// Clone RelationshipType EntityType Mapping Cardinality object
        /// </summary>
        /// <returns>cloned copy of IRelationshipType EntityTypeMapping Cardinality object.</returns>
        new IRelationshipTypeEntityTypeMappingCardinality Clone();

        /// <summary>
        /// Delta Merge of IRelationshipType EntityTypeMapping Cardinality
        /// </summary>
        /// <param name="deltaRelationshipTypeEntityTypeMappingCardinality">IRelationshipType EntityTypeMapping Cardinality that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipType EntityTypeMapping Cardinality instance</returns>
        IRelationshipTypeEntityTypeMappingCardinality MergeDelta(IRelationshipTypeEntityTypeMappingCardinality deltaRelationshipTypeEntityTypeMappingCardinality, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}