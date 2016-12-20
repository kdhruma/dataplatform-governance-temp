using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class Relationship
    {
        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        public JToken EntityId { get; set; }

        /// <summary>
        /// Gets or sets the parent external identifier.
        /// </summary>
        public String ParentEntityId { get; set; }

        /// <summary>
        /// Gets or sets family id representing all variants of one family - EntityFamilyId
        /// </summary>
        public String FamilyId { get; set; }

        /// <summary>
        /// Gets or sets FamilyTreeId represeting entites across catalogs
        /// </summary>
        public String FamilyTreeId { get; set; }

        /// <summary>
        /// Gets or sets family name representing all variants of one family across catalogs
        /// </summary>
        public String FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public String CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the category path.
        /// </summary>
        public String CategoryPath { get; set; }

        /// <summary>
        /// Gets or sets the category name path.
        /// </summary>
        public String CategoryNamePath { get; set; }

        /// <summary>
        /// Gets or sets the variant.
        /// </summary>
        public String Variant { get; set; }

        /// <summary>
        /// Gets or sets the variant path.
        /// </summary>
        public String VariantPath { get; set; }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public String Container { get; set; }

        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        public String ContainerName { get; set; }

        /// <summary>
        /// Gets or sets the container path.
        /// </summary>
        public String ContainerPath { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public String Organization { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        public String Segment { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        public String RelatedExternalId { get; set; }

        /// <summary>
        /// Gets or sets the segment.
        /// </summary>
        public String EntityTypeOfRelatedEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Relationship()
        {
        }
    }
}