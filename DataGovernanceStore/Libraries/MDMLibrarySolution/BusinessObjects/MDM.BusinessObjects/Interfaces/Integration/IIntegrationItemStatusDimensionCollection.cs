using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting collection of integration item dimension status.
    /// </summary>
    public interface IIntegrationItemStatusDimensionCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        Boolean Equals(object obj);

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        Int32 GetHashCode();

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionStatus object
        /// </summary>
        /// <returns>Xml String representing the IntegrationItemDimensionStatusCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of IntegrationItemDimensionStatus collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone IntegrationItemDimensionStatus collection.
        /// </summary>
        /// <returns>cloned IntegrationItemDimensionStatus collection object.</returns>
        IIntegrationItemStatusDimensionCollection Clone();

        /// <summary>
        /// Add status for a dimension value 
        /// </summary>
        /// <param name="dimensionTypeName">ShortName of dimension Type</param>
        /// <param name="dimensionValue">ShortName of dimension Value</param>        
        void Add(String dimensionTypeName, String dimensionValue);

        #endregion Public Methods
    }
}
