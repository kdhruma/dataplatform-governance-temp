using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces.DQM;

    /// <summary>
    /// This Class holds the properties necessary for the Matching Attribute.
    /// </summary>
    [DataContract]
    public class MatchAttribute : IMatchAttribute
    {
        #region Field

        /// <summary>
        /// Field indicates name of match attribute
        /// </summary>
        private String _attributeName = String.Empty;

        /// <summary>
        /// Field indicates locale of match attribute
        /// </summary>
        private LocaleEnum _locale = LocaleEnum.UnKnown;

        /// <summary>
        /// Field indicates collection of values which has to be excluded
        /// </summary>
        private Collection<String> _excludedValues = new Collection<String>();

        #endregion Field

        #region Properties

        /// <summary>
        /// Property denotes name of match attribute
        /// </summary>
        [DataMember]
        public String AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Property denotes locale of match attribute
        /// </summary>
        [DataMember]
        public LocaleEnum Locale
        {
            get { return _locale; }
            set { _locale = value; }
        }

        /// <summary>
        /// Property denotes collection of values which has to be excluded
        /// </summary>
        [DataMember]
        public Collection<String> ExcludedValues
        {
            get { return _excludedValues; }
            set { _excludedValues = value; }
        }

        #endregion Properties

        #region Method

        #endregion Method
    }
}