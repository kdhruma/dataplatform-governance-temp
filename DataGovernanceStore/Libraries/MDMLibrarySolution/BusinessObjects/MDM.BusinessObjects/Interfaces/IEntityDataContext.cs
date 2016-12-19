using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity context, which indicates information to be loaded into the Entity object.
    /// </summary>
    public interface IEntityDataContext
    {
        #region Properties

        /// <summary>
        /// Property denoting
        /// </summary>
        LocaleEnum Locale { get; set; }

        /// <summary>
        /// Property denoting locale collection for entity data.
        /// </summary>
        Collection<LocaleEnum> DataLocales { get; set; }

        /// <summary>
        /// Property denoting whether to load entity properties or not. Here entity properties refers to entity metadata like short name, long name, parent information
        /// </summary>
        Boolean LoadEntityProperties { get; set; }

        /// <summary>
        /// Property denoting whether to load attributes or not.
        /// IF this flag is  to false then even if AttributeIds and group ids are given, no attributes will be loaded.
        /// So if any attributes are required to be fetched then this flag needs to be  to true.
        /// </summary>
        Boolean LoadAttributes { get; set; }

        /// <summary>
        /// Property denoting whether to load relationships or not.
        /// If LoadRelationships = true and no value is given in Relationship Context's RelationshipTypeIdList then all relationships of mapped relationship type will be loaded
        /// If LoadRelationships = true and there are some relationship type ids given in Relationship Context's RelationshipTypeIdList then relationships of given relationship type(s) will be loaded
        /// </summary>
        Boolean LoadRelationships { get; set; }

        /// <summary>
        /// Property denoting whether to load hierarchy relationships or not
        /// </summary>
        Boolean LoadHierarchyRelationships { get; set; }

        /// <summary>
        /// property denoting whether to load extension relationships or not
        /// </summary>
        Boolean LoadExtensionRelationships { get; set; }

        /// <summary>
        /// Property denoting whether to load workflow Information or not.
        /// </summary>
        Boolean LoadWorkflowInformation { get; set; }

        /// <summary>
        /// Property denoting the Workflow Name
        /// </summary>
        String WorkflowName { get; set; }

        /// <summary>
        /// Specifies whether to load state validation attributes or not.
        /// </summary>
        Boolean LoadStateValidationAttributes { get; set; }

        /// <summary>
        /// Specifies whether to load business conditions or not
        /// </summary>
        Boolean LoadBusinessConditions { get; set; }

        #endregion

        #region Methods

        #region AttributeContext Methods

        /// <summary>
        /// Sets the attribute context
        /// </summary>
        /// <param name="attributeContext">Attribute context interface</param>
        void SetAttributeContext(IAttributeContext attributeContext);

        /// <summary>
        /// Gets the attribute context interface
        /// </summary>
        /// <returns>Attribute context interface.</returns>
        IAttributeContext GetAttributeContext();

        #endregion

        #region RelationshipContext Methods

        /// <summary>
        /// Sets the relationship context
        /// </summary>
        /// <param name="iRelationshipContext">Relationship Context Interface</param>
        void SetRelationshipContext(IRelationshipContext iRelationshipContext);

        /// <summary>
        /// Gets the relationship context interface
        /// </summary>
        /// <returns>Relationship Context interface.</returns>
        IRelationshipContext GetRelationshipContext();

        #endregion

        #region EntityHierarchyContext Methods

        /// <summary>
        /// Sets the Entity Hierarchy context
        /// </summary>
        /// <param name="iEntityHierarchyContext">Entity Hierarchy Context Interface</param>
        void SetEntityHierarchyContext(IEntityHierarchyContext iEntityHierarchyContext);

        /// <summary>
        /// Gets the Entity Hierarchy context interface
        /// </summary>
        /// <returns>Entity Hierarchy Context interface.</returns>
        IEntityHierarchyContext GetEntityHierarchyContext();

        #endregion

        #region EntityExtensionContext Methods

        /// <summary>
        /// Sets the Entity Extension context
        /// </summary>
        /// <param name="iEntityExtensionContext">Entity Extension Context Interface</param>
        void SetEntityExtensionContext(IEntityExtensionContext iEntityExtensionContext);

        /// <summary>
        /// Gets the IEntityExtensionContext interface
        /// </summary>
        /// <returns>Entity Extension Context interface.</returns>
        IEntityExtensionContext GetEntityExtensionContext();

        #endregion

        #region Miscelleneous Methods
        
        /// <summary>
        /// Clones IEntityContext object
        /// </summary>
        /// <returns>Returns cloned IEntityContext object</returns>
        IEntityDataContext Clone();
        
        #endregion Miscelleneous Methods

        #endregion
    }
}