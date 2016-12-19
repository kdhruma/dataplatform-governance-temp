using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get attribute model context instance.
    /// </summary>
    public interface IAttributeModelContext
    {
        #region Properties

        /// <summary>
        /// Property denoting model type of an attribute
        /// </summary>
        AttributeModelType AttributeModelType { get; set; }

        /// <summary>
        /// Property denoting container id for the attribute model context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting entity type id for the attribute model context
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting relation type id for the attribute model context
        /// </summary>
        Int32 RelationshipTypeId { get; set; }

        /// <summary>
        /// Property denoting category id for the attribute model context
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting locales in which AttributeModels are to be fetched.
        /// </summary>
        Collection<LocaleEnum> Locales { get; set; }

        /// <summary>
        /// Property denoting EntityId for current context for AttributeModel.
        /// EntityId is used when we want to evaluate the CustomView for some specific Entity.
        /// E.g. Required empty attributes for current entity.
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Property denoting whether to get only attribute models marked as ShowAtCreation
        /// </summary>
        Boolean GetOnlyShowAtCreationAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to get only attribute models marked as Required
        /// </summary>
        Boolean GetOnlyRequiredAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to get complete details or only the main details of attribute model
        /// </summary>
        Boolean GetCompleteDetailsOfAttribute { get; set; }

        /// <summary>
        /// Property denoting whether to sort attribute model or not.
        /// </summary>
        Boolean ApplySorting { get; set; }

        /// <summary>
        /// Property denoting container name for the attribute model context
        /// </summary>
        String ContainerName { get; set; }

        /// <summary>
        /// Property denoting entityType name for the attribute model context
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting category path for the attribute model context
        /// </summary>
        String CategoryPath { get; set; }

        /// <summary>
        /// Property denoting relationshipType name for the attribute model context
        /// </summary>
        String RelationshipTypeName { get; set; }

        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        Boolean LoadStateValidationAttributes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute Model Identifier Context
        /// </summary>
        /// <returns>Xml representation of Attribute Model Data Context</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute Model Identifier Context based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model Data Context</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}