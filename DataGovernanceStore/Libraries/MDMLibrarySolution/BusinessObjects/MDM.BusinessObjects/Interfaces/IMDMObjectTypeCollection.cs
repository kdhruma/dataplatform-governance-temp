using System;
using MDM.BusinessObjects;
using MDM.Core;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods used for determining integration message header containing message context information.
    /// </summary>
    public interface IMDMObjectTypeCollection
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the MDMObjectTypeCollection contains a specific message.
        /// </summary>
        /// <param name="mdmObjectTypeId">The MDMObjectType object to locate in the MDMObjectTypeCollection.</param>
        /// <returns>
        /// <para>true : If MDMObjectType found in MDMObjectTypeCollection</para>
        /// <para>false : If MDMObjectType found not in MDMObjectTypeCollection</para>
        /// </returns>
        Boolean Contains(Int16 mdmObjectTypeId);

        /// <summary>
        /// Remove MDMObjectType object from MDMObjectTypeCollection
        /// </summary>
        /// <param name="mdmObjectTypeId">MDMObjectTypeId of MDMObjectType which is to be removed from collection</param>
        /// <returns>true if MDMObjectType is successfully removed; otherwise, false. This method also returns false if MDMObjectType was not found in the original collection</returns>
        Boolean Remove(Int16 mdmObjectTypeId);

        /// <summary>
        /// Get specific MDMObjectType by Id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of MDMObjectType</param>
        /// <returns><see cref="MDMObjectType"/></returns>
        MDMObjectType GetMDMObjectType(Int16 mdmObjectTypeId);
        
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
        /// Get Xml representation of MDMObjectType object
        /// </summary>
        /// <returns>Xml String representing the MDMObjectTypeCollection</returns>
        String ToXml();
        
        /// <summary>
        /// Get Xml representation of MDMObjectType collection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Clone MDMObjectType collection.
        /// </summary>
        /// <returns>cloned MDMObjectType collection object.</returns>
        IMDMObjectTypeCollection Clone();

        /// <summary>
        /// Gets MDMObjectType by id
        /// </summary>
        /// <param name="mdmObjectTypeId">Id of the MDMObjectType</param>
        /// <returns>MDMObjectType with specified Id</returns>
        IMDMObjectType Get(Int16 mdmObjectTypeId);

        /// <summary>
        /// Gets MDMObjectType by Name
        /// </summary>
        /// <param name="mdmObjectTypeName">Name of the MDMObjectType</param>
        /// <returns>MDMObjectType with specified Id</returns>
        IMDMObjectType Get(String mdmObjectTypeName);

        #endregion Public Methods
    }
}
