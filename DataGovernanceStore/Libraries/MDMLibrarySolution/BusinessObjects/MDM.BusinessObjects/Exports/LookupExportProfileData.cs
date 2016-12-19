using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.Exports
{
    using MDM.Core;
    using MDM.Interfaces.Exports;

    /// <summary>
    /// Specifies the Lookup Export Profile Data base Object
    /// </summary>
    [DataContract]
    [KnownType(typeof(LookupExportSyndicationProfileData))]
    [Serializable]
    public abstract class LookupExportProfileData : MDMObject, ILookupExportProfileData
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupExportProfileData"/> class. 
        /// Parameter-less Constructor
        /// </summary>
        public LookupExportProfileData()
            : base()
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Represents LookupExportProfileData object in Xml format
        /// </summary>
        /// <returns>String representation of current  LookupExportProfileData object</returns>
        public override abstract String ToXml();

        /// <summary>
        /// Represents LookupExportProfileData object in Xml format
        /// </summary>
        /// <param name="objectSerialization">Specifies the object serialization type</param>
        /// <returns>String representation of current LookupExportProfileData object</returns>
        public override abstract String ToXml(ObjectSerialization objectSerialization);

        #endregion

        #endregion
    }
}

