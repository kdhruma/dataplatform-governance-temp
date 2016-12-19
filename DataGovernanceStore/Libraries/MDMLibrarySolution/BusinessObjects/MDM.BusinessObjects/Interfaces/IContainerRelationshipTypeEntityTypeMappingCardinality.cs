using MDM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the container relationship type entity type cardinality mapping related information.
    /// </summary>
    public interface IContainerRelationshipTypeEntityTypeMappingCardinality : IRelationshipTypeEntityTypeMappingCardinality
    {
        #region Properties

        /// <summary>
        /// Property denoting the Organization Id
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Property denoting the Organization Name
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Property denoting the Container Id
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting the Container Name
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Specifies if mapping is specialized for container or derived from entity type and relationship type mapping. 
        /// </summary>
        Boolean IsSpecialized { get; set; }

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
        new IContainerRelationshipTypeEntityTypeMappingCardinality Clone();

        /// <summary>
        /// Delta Merge of IRelationshipType EntityTypeMapping Cardinality
        /// </summary>
        /// <param name="deltaContainerRelationshipTypeEntityTypeMappingCardinality">IRelationshipType EntityTypeMapping Cardinality that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged IRelationshipType EntityTypeMapping Cardinality instance</returns>
        IContainerRelationshipTypeEntityTypeMappingCardinality MergeDelta(IContainerRelationshipTypeEntityTypeMappingCardinality deltaContainerRelationshipTypeEntityTypeMappingCardinality, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}