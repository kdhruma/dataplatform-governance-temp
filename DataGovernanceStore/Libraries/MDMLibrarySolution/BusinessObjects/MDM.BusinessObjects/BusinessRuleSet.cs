using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

using MDM.Core;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies a BusinessRuleSet
    /// </summary>
    [DataContract]
    public class BusinessRuleSet : MDMObject
    {
        #region Fields

        /// <summary>
        /// Field denoting if business rule set is Enabled
        /// </summary>
        private Boolean _enabled = false;

        /// <summary>
        /// Field denoting PropertyXML of business rule set
        /// </summary>
        private String _propertyXml = String.Empty;

        /// <summary>
        /// Field denoting Collection of BusinessRule
        /// </summary>
        private Collection<BusinessRule> _businessRules = new Collection<BusinessRule>();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        /// 
        public BusinessRuleSet()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleSet</param>
        public BusinessRuleSet(Int32 id)
            :base(id)
        {

        }

        /// <summary>
        /// Constructor with Id, Name and Description of a BusinessRuleSet as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleSet</param>
        /// <param name="name">Indicates the ShortName of a BusinessRuleSet </param>
        /// <param name="longName">Indicates the LongName of a BusinessRuleSet </param>
        public BusinessRuleSet(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Description of a BusinessRuleSet as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of a BusinessRuleSet</param>
        /// <param name="name">Indicates the ShortName of a BusinessRuleSet </param>
        /// <param name="longName">Indicates the LongName of a BusinessRuleSet </param>
        /// <param name="locale">Indicates the Locale of a BusinessRuleSet </param>
        public BusinessRuleSet(Int32 id, String name, string longName, LocaleEnum locale)
            :base(id,name,longName,locale)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting if BusinessRuleSet is Enabled 
        /// </summary>
        [DataMember]
        public Boolean Enabled
        {
            get { return this._enabled; }
            set { this._enabled = value; }
        }

        /// <summary>
        /// Property denoting the PropertyXml of BusinessRuleSet
        /// </summary>
        [DataMember]
        public String PropertyXml
        {
            get { return this._propertyXml; }
            set { this._propertyXml = value; }
        }

        /// <summary>
        /// Property denoting the Collection of BusinessRules in a BusinessRuleSet
        /// </summary>
        [DataMember]
        public Collection<BusinessRule> BusinessRules
        {
            get { return this._businessRules; }
            set { this._businessRules = value; }
        }

        #endregion

    }
}
