using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get MDM object types such as Entity and Entity Collection. 
    /// </summary>
    public interface IMDMObjectType
    {
        #region Properties

        /// <summary>
        /// Indicates Id of MDMObjectType
        /// </summary>
        Int16 Id { get; set; }

        /// <summary>
        /// Indicates name of MDMObjectType. E.g., Entity, Relationship
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Indicates fully qualified class name of MDMObject
        /// </summary>
        String ClassName { get; set; }

        /// <summary>
        /// Indicates Assembly in which specified object is contained.
        /// </summary>
        String AssemblyName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get xml version of MDMObjectType
        /// </summary>
        /// <returns>Xml representation of MDMObjectType object</returns>
        String ToXml();

        /// <summary>
        /// Create a new instance of MDMObjectType with same value as current one.
        /// </summary>
        /// <returns><see cref="IMDMObjectType"/></returns>
        IMDMObjectType Clone();

        #endregion Methods
    }
}
