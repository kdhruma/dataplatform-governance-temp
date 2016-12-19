using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get impacted entity related information.
    /// </summary>
    public interface IImpactedEntity : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property for the id of an Entity
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Field for the id of impacted Entity
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Indicates the priority of the entity from the processing point of view.
        /// </summary>
        Int32 Priority { get; set; }

        /// <summary>
        /// Indicates if a given entity needs to be updated in the cache or not.
        /// </summary>
        Boolean IsCacheDirty { get; set; }

        /// <summary>
        /// Indicates if the entity denorm has to be processed for the given entity.
        /// </summary>
        Boolean IsEntityDenormRequired { get; set; }

        /// <summary>
        /// Contains the list of a attribute ids which got impacted.
        /// </summary>
        Collection<Int32> ImpactedAttributes { get; set; }

        /// <summary>
        /// Contains the list of a locales for impacted attributes.
        /// </summary>
        Collection<LocaleEnum> ImpactedAttributeLocales { get; set; }

        /// <summary>
        /// Indicates if the entity denorm is currently in progress.
        /// </summary>
        Boolean IsEntityDenormInProcess { get; set; }

        /// <summary>
        /// When the attribute denorm is in progress, this attribute list is updated in this field.
        /// </summary>
        Collection<Int32> ShelvedAttributes { get; set; }

        /// <summary>
        /// When the attribute denorm is currently in progress, the locales are updated in this field
        /// </summary>
        Collection<LocaleEnum> ShelvedAttributeLocales { get; set; }

        /// <summary>
        /// Contains the list of impacted relationship ids 
        /// </summary>
        Collection<Int64> ImpactedRelationships { get; set; }

        /// <summary>
        /// Contains the list of locales for impacted relationships
        /// </summary>
        Collection<LocaleEnum> ImpactedRelationshipLocales { get; set; }

        /// <summary>
        /// When the Relationship denorm is in progress, this relationship list is updated in this field
        /// </summary>
        Collection<Int64> ShelvedRelationships{ get; set; }

        /// <summary>
        /// When the Relationship denorm is currently in progress, the locales are updated in this field.
        /// </summary>
        Collection<LocaleEnum> ShelvedRelationshipLocales { get; set; }

        #endregion Properties
    }
}

