using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for providing category related information.
    /// </summary>
    public interface ICategory : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates the Id of a category
        /// </summary>
        new Int64 Id { get; }

        /// <summary>
        /// Indicates the long name of a category
        /// </summary>
        new String LongName { get; }

        /// <summary>
        /// Indicates the Id of hierarchy under which category resides
        /// </summary>
        Int32 HierarchyId { get; }

        /// <summary>
        /// Indicates the name of hierarchy
        /// </summary>
        String HierarchyName { get; }

        /// <summary>
        /// Indicates the long name of hierarchy
        /// </summary>
        String HierarchyLongName { get; }

        /// <summary>
        /// Indicates the object type
        /// </summary>
        String ObjectType { get; }

        /// <summary>
        /// Indicates the level of category
        /// </summary>
        Int32 Level { get; }

        /// <summary>
        /// Indicates the category path
        /// </summary>
        String Path { get; }

        /// <summary>
        /// Indicates the long name path
        /// </summary>
        String LongNamePath { get; }

        /// <summary>
        /// Indicates whether the category is at leaf level or not
        /// </summary>
        Boolean IsLeaf { get; }

        /// <summary>
        /// Indicates the parent category Id
        /// </summary>
        Int64 ParentCategoryId { get; }
        
        #endregion

        #region Methods

        /// <summary>
        /// Clones category object
        /// </summary>
        /// <returns>cloned copy of category object.</returns>
        ICategory Clone();

        /// <summary>
        /// Merges of delta category
        /// </summary>
        /// <param name="deltaCategory">Category that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged category instance</returns>
        ICategory MergeDelta(ICategory deltaCategory, ICallerContext iCallerContext, Boolean returnClonedObject = true);


        /// <summary>
        /// Gets Xml representation of category
        /// </summary>
        /// <returns>Xml representation of Category</returns>
        String ToXml();

        /// <summary>
        /// Gets Xml representation of category based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Category</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}