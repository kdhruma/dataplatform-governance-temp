using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity variant level.
    /// </summary>
    public interface IEntityVariantLevel : IMDMObject
    {
        /// <summary>
        /// Property denoting entity type Id
        /// </summary>
        Int32 EntityTypeId { get; set; }
        
        /// <summary>
        /// Field denoting entity type name of current variant level
        /// </summary>
       String EntityTypeName { get; set; }

        /// <summary>
        /// Field denoting entity type long name of current variant level
        /// </summary>
        String EntityTypeLongName { get; set; }

        /// <summary>
        /// Property denoting Rank of a level
        /// </summary>
        Int32 Rank { get; set; }

        /// <summary>
        /// Append dimension attribute in current attribute collection
        /// </summary>
        /// <param name="iAttribute">Indicates the dimension attribute</param>
        void AppendDimensionAttribute(IAttribute iAttribute);

        /// <summary>
        /// Set Dimension Values
        /// </summary>
        /// <param name="iTable">Indicates the dimension values in a form of table</param>
        void SetDimensionValues(ITable iTable);

        /// <summary>
        /// Append Rule Attribute
        /// </summary>
        /// <param name="iAttribute">Indicates Rule Attribute</param>
        /// <param name="targetAttrId">Indicates Target attribute Id</param>
        /// <param name="isOptional">Indicates Append rule attribute is optional or not</param>
        void AppendRuleAttribute(IAttribute iAttribute, Int32 targetAttrId, Boolean isOptional);
        
        /// <summary>
        /// Get Xml representation of entity variant level object
        /// </summary>
        /// <param name="needValues">Represents a boolean value indicating if attribute value details are required in xml representation.</param>
        /// <returns>Returns Xml representation of entity variant level object</returns>
        String ToXml(Boolean needValues);
    }
}
