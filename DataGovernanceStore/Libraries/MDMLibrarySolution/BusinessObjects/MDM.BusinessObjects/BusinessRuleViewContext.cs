using System;
using System.Runtime.Serialization;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the Type of a BusinessRuleViewContext
    /// </summary>
    [DataContract]
    public class BusinessRuleViewContext : MDMObject
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// 
        public BusinessRuleViewContext()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleViewContext</param>
        public BusinessRuleViewContext(Int32 id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a BusinessRuleViewContext as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleViewContext</param>
        /// <param name="name">Indicates the ShortName of a BusinessRuleViewContext </param>
        /// <param name="longName">Indicates the LongName of a BusinessRuleViewContext </param>
        public BusinessRuleViewContext(Int32 id, String name, String longName)
            : base(id,name,longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName and Locale of an BusinessRuleViewContext as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleViewContext</param>
        /// <param name="name">Indicates the ShortName of a BusinessRuleViewContext </param>
        /// <param name="longName">Indicates the LongName of a BusinessRuleViewContext </param>
        /// <param name="locale">Indicates the Locale of a BusinessRuleViewContext </param>
        public BusinessRuleViewContext(Int32 id, String name, String longName, LocaleEnum locale)
            : base(id, name, longName, locale)
        {
            
        }

        #endregion

        #region Properties

        #endregion
    }
}
