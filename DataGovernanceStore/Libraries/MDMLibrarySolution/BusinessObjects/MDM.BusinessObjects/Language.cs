﻿using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Language
    /// </summary>
    [DataContract]
    public class Language : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Language()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a Language</param>
        public Language(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a Language as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Language</param>
        /// <param name="name">Indicates the Name of a Language</param>
        /// <param name="longName">Indicates the Description of a Language</param>
        public Language(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of a Language as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a Language</param>
        /// <param name="name">Indicates the Name of a Language</param>
        /// <param name="longName">Indicates the LongName of a Language</param>
        /// <param name="locale">Indicates the Locale of a Language</param>
        public Language(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        #endregion

        #region Properties

        #endregion

    }
}
