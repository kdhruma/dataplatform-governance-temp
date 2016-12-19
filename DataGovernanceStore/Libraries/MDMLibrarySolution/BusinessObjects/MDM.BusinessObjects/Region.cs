using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Region
    /// </summary>
    [DataContract]
    public class Region : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Region()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Region</param>
        public Region(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Region as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Region</param>
        /// <param name="name">Indicates the Name of a Region</param>
        /// <param name="longName">Indicates the Description of a Region</param>
        public Region(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Region as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Region</param>
        /// <param name="name">Indicates the Name of a Region</param>
        /// <param name="longName">Indicates the LongName of a Region</param>
        /// <param name="locale">Indicates the Locale of a Region</param>
        public Region(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        #endregion

        #region Properties

        #endregion

    }
}
