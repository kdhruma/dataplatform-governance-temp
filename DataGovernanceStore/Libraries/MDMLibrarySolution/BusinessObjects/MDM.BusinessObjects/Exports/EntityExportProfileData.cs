using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Entity Export Profile data base object.
    /// </summary>
    [DataContract]
    [KnownType(typeof(EntityExportSyndicationProfileData))]
    [KnownType(typeof(EntityExportUIProfileData))]
    [KnownType(typeof(MDMObject))]
    [Serializable]
    public abstract class EntityExportProfileData : MDMObject, IEntityExportProfileData
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExportProfileData"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public EntityExportProfileData()
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents syndication exportprofiledata in Xml format
        /// </summary>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        public override abstract String ToXml();

        /// <summary>
        /// Represents syndication exportprofiledata in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current syndication exportprofiledata object</returns>
        public override abstract String ToXml(ObjectSerialization objectSerialization);

        #endregion

        #endregion
    }
}
