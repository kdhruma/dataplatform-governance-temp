using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Type of an Organization
    /// </summary>
    [DataContract]
    public class OrganizationType : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public OrganizationType()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization Type</param>
        public OrganizationType(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of an Organization Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization Type</param>
        /// <param name="name">Indicates the Name of an Organization Type</param>
        /// <param name="longName">Indicates the Description of an Organization Type</param>
        public OrganizationType(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an Organization Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Organization Type</param>
        /// <param name="name">Indicates the Name of an Organization Type</param>
        /// <param name="longName">Indicates the LongName of an Organization Type</param>
        /// <param name="locale">Indicates the Locale of an Organization Type</param>
        public OrganizationType(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        #endregion

        #region Properties

        #endregion

    }
}
