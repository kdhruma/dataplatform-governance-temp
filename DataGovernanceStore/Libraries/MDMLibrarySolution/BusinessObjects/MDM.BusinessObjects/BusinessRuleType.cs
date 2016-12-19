using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects
{

    /// <summary>
    /// Specifies the Type of a BusinessRule
    /// </summary>
    [DataContract]
    public class BusinessRuleType : MDMObject
    {
        #region Fields

        /// <summary>
        /// field denoting IsValueRule
        /// </summary>
        private Boolean _isValueRule=false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public BusinessRuleType()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule Type</param>
        public BusinessRuleType(Int32 id)
            :base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a BusinessRule Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule Type</param>
        /// <param name="name">Indicates the ShortName of a BusinessRule Type</param>
        /// <param name="longName">Indicates the LongName of a BusinessRule Type</param>
        public BusinessRuleType(Int32 id, String name, String longName)
            :base(id,name,longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an BusinessRule Type as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRule Type</param>
        /// <param name="name">Indicates the ShortName of a BusinessRule Type</param>
        /// <param name="longName">Indicates the LongName of a BusinessRule Type</param>
        /// <param name="locale">Indicates the Locale of a BusinessRule Type</param>
        public BusinessRuleType(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting if BusinessRule Type is of "ValueRule"
        /// </summary>
        [DataMember]
        public Boolean IsValueRule
        {
            get { return this._isValueRule; }
            set { this._isValueRule = value; }
        }

        #endregion
    }
}
