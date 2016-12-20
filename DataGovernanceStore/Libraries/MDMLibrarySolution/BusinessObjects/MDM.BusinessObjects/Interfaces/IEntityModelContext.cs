using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get entity model context details
    /// </summary>
    public interface IEntityModelContext
    {
        /// <summary>
        /// Specifies the Organization Id for the entity model
        /// </summary>
        Int32 OrganizationId { get; set; }

        /// <summary>
        /// Specifies the container Id for the entity model
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Specifies the hierarchy Id for the entity model
        /// </summary>
        Int32 HierarchyId { get; set; }

        /// <summary>
        /// Specifies the entity type Id for the entity model
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Specifies the category Id for the entity model
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Specifies the List of relationship type Id for the entity model
        /// </summary>
        Collection<Int32> RelationshipTypeIds { get; set; }

        /// <summary>
        /// Specifies the organization short name for the entity model
        /// </summary>
        String OrganizationName { get; set; }

        /// <summary>
        /// Specifies the container short name for the entity model
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Specifies the hierarchy short name for the entity model
        /// </summary>
        String HierarchyName { get; set; }

        /// <summary>
        /// Specifies the entity type short name for the entity model
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Specifies the category path for the entity model
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Specifies the list of relationship type short name for the entity model
        /// </summary>
        Collection<String> RelationshipTypeNames { get; set; }
      
        /// <summary>
        ///  Clones the EntityModelContext
        /// </summary>
        /// <returns>Cloned EntityModelContext object</returns>
        IEntityModelContext Clone();
    }
}