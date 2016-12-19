using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Culture
    /// </summary>
    [DataContract]
    public class Culture : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Culture()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Culture</param>
        public Culture(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Culture as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Culture</param>
        /// <param name="name">Indicates the Name of a Culture</param>
        /// <param name="longName">Indicates the Description of a Culture</param>
        public Culture(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Culture as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Culture</param>
        /// <param name="name">Indicates the Name of a Culture</param>
        /// <param name="longName">Indicates the LongName of a Culture</param>
        /// <param name="locale">Indicates the Locale of a Culture</param>
        public Culture(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        
        }

        #endregion

        #region Properties

        #endregion

    }
}
