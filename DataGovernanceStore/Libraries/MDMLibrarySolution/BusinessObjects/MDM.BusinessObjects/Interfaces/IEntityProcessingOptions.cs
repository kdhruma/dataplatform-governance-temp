using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get entity processing options, which specifies various flags and indications to entity processing logic.
    /// </summary>
    public interface IEntityProcessingOptions
    {
        #region Properties

        /// <summary>
        /// Property indicates if validation process to be executed as part of entity processing
        /// </summary>
        Boolean ValidateEntities
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates if events to be published as part of entity processing
        /// </summary>
        Boolean PublishEvents
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates to process only entities without attributes and relationships
        /// </summary>
        Boolean ProcessOnlyEntities
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates if default values need to be processed
        /// </summary>
        Boolean ProcessDefaultValues
        {
            get;
        }

        /// <summary>
        /// Property indicates collection processing type
        /// </summary>
        CollectionProcessingType CollectionProcessingType
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property denoting whether the attribute level partial Processing is Enable or not.
        /// </summary>
        Boolean IsPartialAttributeprocessingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting whether the relationship attribute level partial Processing is Enable or not.
        /// </summary>
        Boolean IsPartialRelationshipAttributeProcessingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting whether the relationship type level partial Processing is Enable or not.
        /// </summary>
        Boolean IsPartialRelationshipTypeProcessingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting list of Attribute Ids needs to be Validate.
        /// </summary>
        Collection<Int32> ValidationMustOnAttributeIdList
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting list of RelationshipType Ids needs be Validate. 
        /// </summary>
        Collection<Int32> ValidationMustOnRelationshipTypeIdList
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting list of Relationship Attribute Ids needs to be Validate.
        /// </summary>
        Collection<Int32> ValidationMustOnRelationshipAttributeIdList
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates if invalid data are allowed in the system. If this is set to true, even if AttributeModel validation fails for attribute, it will be allowed to set.
        /// </summary>
        Boolean AllowInvalidData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        AttributeProcessingOptionsCollection AttributeProcessingOptionCollection { get; set; }

        /// <summary>
        /// Property indicates if system need to ignore case sensitive comparision while identifing changes
        /// </summary>
        Boolean IgnoreCase
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the procession options on whether matching is required or not.
        /// </summary>
        MatchProcessingOptions MatchProcessingOptions { get; set; }

        /// <summary>
        /// Indicates if the match store needs to be updated for this entity. If the value is true and if the attributes that got changed belong to a store, then a job will be queued for the 
        /// match store load processor to process this delta change.
        /// </summary>
        Boolean LoadMatchStore { get; set; }

        /// <summary>
        /// Property defining the behavior of the attribute compare and merge. This behavior is applicable for common, technical and relationship attributes
        /// </summary>
        AttributeCompareAndMergeBehavior AttributeCompareAndMergeBehavior
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents EntityProcessingOptions in Xml format
        /// </summary>
        /// <returns>String representation of current EntityProcessingOptions object instance</returns>
        String ToXml();

        #endregion Methods

    }
}
