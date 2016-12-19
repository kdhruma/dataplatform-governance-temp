using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get attribute model collection object.
    /// </summary>
    public interface IAttributeModelCollection : IEnumerable<AttributeModel>
    {
        #region Properties

        /// <summary>
        /// Returns No. of AttributeModel Instance in to the current IAttributeModelCollection
        /// </summary>
        Int32 Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute Model Collection
        /// </summary>
        /// <returns>Xml representation of Attribute Model Collection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute Model Collection based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute Model Collection</returns>
        String ToXml(ObjectSerialization objectSerialization);

        /// <summary>
        /// Get attribute id array from the current attribute model collection
        /// </summary>
        /// <returns>Array of Int32 of attributes ids in current attribute model collection</returns>
        /// <exception cref="Exception">Thrown if there are no attributes in current attribute model collection</exception>
        Collection<Int32> GetAttributeIdList();

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeId and Locale
        /// </summary>
        /// <param name="attributeId">Id of an AttributeModel to search in</param>
        /// <param name="locale">Locale of an AttributeModel to search in AttributeModelCollection.</param>
        /// <returns>AttributeModel having given AttributeId and Locale</returns>
        IAttributeModel GetAttributeModel(Int32 attributeId, LocaleEnum locale);

        /// <summary>
        /// Get AttributeModel from AttributeModelCollection based on AttributeUniqueIdentifier (Attribute Name + attribute Parent Name) and Locale
        /// </summary>
        /// <param name="iAttributeUniqueIdentifier">AttributeUniqueIdentifier to search in AttributeModelCollection</param>
        /// <param name="locale">Locale of an AttributeUniqueIdentifier to search in AttributeModelCollecton.</param>
        /// <returns>AttributeModel having given AttributeUniqueIdentifier and Locale</returns>
        IAttributeModel GetAttributeModel(IAttributeUniqueIdentifier iAttributeUniqueIdentifier, LocaleEnum locale);

        /// <summary>
        /// Add attribute models in collection
        /// </summary>
        /// <param name="iAttributeModels">attribute models to add in collection</param>
        void AddRange(IAttributeModelCollection iAttributeModels);

        /// <summary>
        /// Add attribute model in collection
        /// </summary>
        /// <param name="iAttributeModel">Attribute model to add in collection</param>
        void Add(IAttributeModel iAttributeModel);

        /// <summary>
        /// Removes the first occurrence of a specific object from the Attribute model collection
        /// </summary>
        /// <param name="iAttributeModel">The object to remove from the Attribute Model collection</param>
        /// <returns>true if item is successfully removed; otherwise, false</returns>
        Boolean Remove(IAttributeModel iAttributeModel);

        /// <summary>
        /// Marks all the attribute models as readOnly
        /// </summary>
        void MarkAsReadOnly();

        /// <summary>
        /// Get All Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>Common AttributeModelCollection</returns>
        IAttributeModelCollection GetCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required Common AttributeMdodels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required and ReadOnly Common AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredReadOnlyCommonAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required and ReadOnly Technical AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredReadOnlyTechnicalAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Get All Required and ReadOnly Relationship AttributeModels available in Entity
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetRequiredReadOnlyRelationshipAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets Show At Creation Attribute Models
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetShowAtCreationAttributeModels(LocaleEnum locale);

        /// <summary>
        /// Gets AttributeModels which have default values configured
        /// </summary>
        /// <param name="locale">Specifies in which locale AttributeModels should be returned</param>
        /// <returns>AttributeModels</returns>
        IAttributeModelCollection GetAttributeModelsWithDefaultValues(LocaleEnum locale);

        #endregion
    }
}
