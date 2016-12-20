using System;

namespace MDM.Interfaces.EntityIdentification
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity Identifier map.
    /// </summary>
    public interface IEntityUniqueIdentificationMap
    {
        #region Properties

        /// <summary>
        /// Property denoting id of the entity map used for internal operations
        /// </summary>
        Int64 Id { get; set; }

        /// <summary>
        /// Property denoting entity external Id
        /// </summary>
        String ExternalId { get; set; }

        /// <summary>
        /// Property denoting container Id of the entity in the context
        /// </summary>
        Int32 ContainerId { get; set; }

        /// <summary>
        /// Property denoting category Id of the entity in the context
        /// </summary>
        Int64 CategoryId { get; set; }

        /// <summary>
        /// Property denoting entity type Id of the entity in the context
        /// </summary>
        Int32 EntityTypeId { get; set; }

        /// <summary>
        /// Property denoting entity identifier 1
        /// </summary>
        String EntityIdentifier1Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 2
        /// </summary>
        String EntityIdentifier2Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 3
        /// </summary>
        String EntityIdentifier3Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 4
        /// </summary>
        String EntityIdentifier4Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier 5
        /// </summary>
        String EntityIdentifier5Value { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 1
        /// </summary>
        Int64 EntityIdentifier1Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 2
        /// </summary>
        Int64 EntityIdentifier2Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 3
        /// </summary>
        Int64 EntityIdentifier3Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 4
        /// </summary>
        Int64 EntityIdentifier4Result { get; set; }

        /// <summary>
        /// Property denoting entity identifier result 5
        /// </summary>
        Int64 EntityIdentifier5Result { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Identifier Map
        /// </summary>
        /// <returns>Xml representation of Entity Identifier Map</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity Identifier Map based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity Identifier map</returns>
        String ToXml(ObjectSerialization objectSerialization);


        /// <summary>
        /// Evaluates if any Entity identifier result exists
        /// </summary>
        /// <returns>true|false</returns>
        Boolean HasAnyResults();
        #endregion
    }
}