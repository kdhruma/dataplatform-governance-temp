using System;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity variant definition.
    /// </summary>
    public interface IEntityVariantDefinition : IMDMObject
    {
        /// <summary>
        /// Property denoting root entity type identifier
        /// </summary>
        Int32 RootEntityTypeId { get; set; }

        /// <summary>
        /// Property denoting root entity type name
        /// </summary>
        String RootEntityTypeName { get; set; }

        /// <summary>
        /// Property denoting root entity type long name
        /// </summary>
        String RootEntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting entity variant context identifier
        /// </summary>
        Int32 ContextId { get; set; }

        /// <summary>
        /// Specifies whether dimension attributes are specified or not
        /// </summary>
        Boolean HasDimensionAttributes { get; set; }

        /// <summary>
        /// Property denoting levels of the entity variant
        /// </summary>
        EntityVariantLevelCollection EntityVariantLevels { get; set; }

        /// <summary>
        /// Add entity variant level in entity variant definition
        /// </summary>
        /// <param name="iEntityVariantLevel">Indicates the entity variant level to append to definition</param>
        void AppendEntityVariantLevel(IEntityVariantLevel iEntityVariantLevel);

        /// <summary>
        /// Get Xml representation of entity variant definition object
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns Xml representation of entity variant definition object</returns>
        String ToXml(Boolean needValues);
    }
}