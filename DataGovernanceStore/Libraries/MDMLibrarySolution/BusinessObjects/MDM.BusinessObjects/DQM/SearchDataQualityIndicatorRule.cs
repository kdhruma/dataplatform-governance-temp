using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    ///<summary>
    /// Specifies SearchDataQualityIndicatorRule
    ///</summary>
    [DataContract]
    public class SearchDataQualityIndicatorRule : ISearchDataQualityIndicatorRule
    {
        #region Fields

        /// <summary>
        /// Specifies the Id of DataQualityIndicator for which rule is defined
        /// </summary>
        private Int16 _dataQualityIndicatorId;

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;

        /// <summary>
        /// Represents rule value for search
        /// </summary>
        private Boolean? _value;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the Id of DataQualityIndicator for which rule is defined
        /// </summary>
        [DataMember]
        public Int16 DataQualityIndicatorId
        {
            get
            {
                return _dataQualityIndicatorId;
            }
            set
            {
                _dataQualityIndicatorId = value;
            }
        }

        /// <summary>
        /// Represents rule operator for search
        /// </summary>
        [DataMember]
        public SearchOperator Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        /// <summary>
        /// Represents rule value for search
        /// </summary>
        [DataMember]
        public Boolean? Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether two Object instances are equal
        /// </summary>
        /// <param name="obj">The Object to compare with the current Object</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false</returns>
        public override Boolean Equals(object obj)
        {
            if (obj != null)
            {
                SearchDataQualityIndicatorRule objectToBeCompared = obj as SearchDataQualityIndicatorRule;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    return
                        this.DataQualityIndicatorId == objectToBeCompared.DataQualityIndicatorId &&
                        this.Operator == objectToBeCompared.Operator &&
                        this.Value == objectToBeCompared.Value;
                }
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for type
        /// </summary>
        /// <returns>A hash code for the current Object</returns>
        public override Int32 GetHashCode()
        {
            return
                this.DataQualityIndicatorId.GetHashCode()
                ^ this.Operator.GetHashCode()
                ^ this.Value.GetHashCode();
        }

        #endregion
    }
}