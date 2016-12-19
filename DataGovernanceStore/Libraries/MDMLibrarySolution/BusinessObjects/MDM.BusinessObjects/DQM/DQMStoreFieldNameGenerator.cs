using System;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;

    /// <summary>
    /// Represents class for DQM store field name generator
    /// </summary>
    public sealed class DQMStoreFieldNameGenerator
    {
        /// <summary>
        /// Forms the field name for the attribute with locale
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetStoreFieldName(Int32 attributeId, LocaleEnum locale)
        {
            return String.Format("{0}_{1}", attributeId, (Int32)locale);
        }

        /// <summary>
        /// Forms the field name for the attribute with locale
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetStoreFieldName(String attributeId, LocaleEnum locale)
        {
            return String.Format("{0}_{1}", attributeId, (Int32)locale);
        }

        /// <summary>
        /// Forms the field name for the attribute with locale and sequence number
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="sequence"></param>
        /// <param name="locale"></param>
        /// <returns></returns>
        public static String GetStoreFieldNameForCollection(Int32 attributeId, Int32 sequence, LocaleEnum locale)
        {
            return String.Format("{0}_{1}_{2}", attributeId, sequence, (Int32)locale);
        }

        /// <summary>
        /// Forms the field name for the organization id.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForOrganizationId()
        {
            return "OrganizationId";
        }

        /// <summary>
        /// Forms the field name for the container.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForContainerId()
        {
            return "ContainerId";
        }

        /// <summary>
        /// Forms the field name for the entity type.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForEntityTypeId()
        {
            return "EntityTypeId";
        }

        /// <summary>
        /// Forms the field name for the category id.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForCategoryId()
        {
            return "CategoryId";
        }

        /// <summary>
        /// Forms the field name for the entity id.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForEntityId()
        {
            return "EntityId";
        }

        /// <summary>
        /// Forms the field name for the short name.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForShortName()
        {
            return "ShortName";
        }

        /// <summary>
        /// Forms the field name for the Long Name.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForLongName()
        {
            return "LongName";
        }

        /// <summary>
        /// Forms the field name for the variable.
        /// </summary>
        /// <returns></returns>
        public static String GetStoreFieldNameForVariableAttribute()
        {
            return "Variable";
        }
    }
}
