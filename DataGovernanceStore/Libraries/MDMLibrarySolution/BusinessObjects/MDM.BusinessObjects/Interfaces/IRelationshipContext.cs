using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the relationship context.
    /// </summary>
    public interface IRelationshipContext
    {
        #region Properties

        /// <summary>
        /// Property denoting the child relationships
        /// </summary>
        Collection<Int32> RelationshipTypeIdList { get; set; }

        /// <summary>
        /// Property denoting the relationship type name list
        /// </summary>
        Collection<String> RelationshipTypeNames { get; set; }

        /// <summary>
        /// Property denoting whether to load Relationship attributes or not
        /// </summary>
        Boolean LoadRelationshipAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to load related entities attributes or not
        /// </summary>
        Boolean LoadRelatedEntitiesAttributes { get; set; }

        /// <summary>
        /// Property denoting which attributes are to be loaded.
        /// To load attributes from attribute ids given in related entities attributeIdList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        Collection<Int32> RelatedEntitiesAttributeIdList { get; set; }

        /// <summary>
        /// Field denoting which attributes are to be loaded.
        /// To load attributes from attribute name and group name given in related entities attributeNameGroupNameList, LoadRelatedEntitiesAttributes should be set to true.
        /// </summary>
        Collection<String> RelatedEntitiesAttributeNames { get; set; }

        /// <summary>
        /// Property denoting locale
        /// </summary>
        LocaleEnum Locale { get; set; }

        /// <summary>
        /// Property denoting list of data locales
        /// </summary>
        Collection<LocaleEnum> DataLocales { get; set; }

        /// <summary>
        /// Field denoting whether to load relationship attribute models or not
        /// </summary>
        Boolean LoadRelationshipAttributeModels { get; set; }

        /// <summary>
        /// Property denoting whether to load relationships attributes having blank values / no value
        /// When set to true, relationship get will return attribute object instances having blank / no value
        /// </summary>
        Boolean LoadBlankRelationshipAttributes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents RelationshipContext  in XML format
        /// </summary>
        /// <returns>String representation of current RelationshipContext object</returns>
        String ToXml();

        /// <summary>
        /// Adds attribute name in RelatedEntitiesAttributeNames
        /// </summary>
        /// <param name="attributeName">Represents Attribute short name</param>
        void AddRelatedEntitiesAttributeName(String attributeName);

        #endregion
    }
}