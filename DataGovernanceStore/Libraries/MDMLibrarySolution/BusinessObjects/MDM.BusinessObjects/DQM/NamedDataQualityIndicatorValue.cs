using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    /// <summary>
    /// Specifies Named DataQualityIndicator Value
    /// </summary>
    [DataContract]
    public class NamedDataQualityIndicatorValue : DataQualityIndicatorValue
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public NamedDataQualityIndicatorValue(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NamedDataQualityIndicatorValue(String name, Boolean? value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Construct NamedDataQualityIndicatorValue from DataQualityIndicatorValue
        /// </summary>
        public NamedDataQualityIndicatorValue(DataQualityIndicatorValue dataQualityIndicator, String name)
            : base(dataQualityIndicator)
        {
            this.Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting Name
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        #endregion
    }
}
