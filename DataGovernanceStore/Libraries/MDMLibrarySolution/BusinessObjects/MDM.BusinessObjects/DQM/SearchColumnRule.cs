using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Core;
    using MDM.Interfaces;

    ///<summary>
    /// Specifies SearchColumnRule
    ///</summary>
    [DataContract]
    public class SearchColumnRule : ISearchColumnRule
    {
        #region Fields

        /// <summary>
        /// Specifies the ColumnName for which rule is defined
        /// </summary>
        private String _columnName;

        /// <summary>
        /// Represents search operator for rule
        /// </summary>
        private SearchOperator _operator = SearchOperator.None;

        /// <summary>
        /// Represents rule value for search
        /// </summary>
        private String _value;

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the ColumnName for which rule is defined
        /// </summary>
        [DataMember]
        public String ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
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
        public String Value
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
                SearchColumnRule objectToBeCompared = obj as SearchColumnRule;
                if (objectToBeCompared != null)
                {
                    if (ReferenceEquals(this, objectToBeCompared))
                    {
                        return true;
                    }

                    return
                        this.ColumnName == objectToBeCompared.ColumnName &&
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
                this.ColumnName.GetHashCode()
                ^ this.Operator.GetHashCode()
                ^ this.Value.GetHashCode();
        }

        #endregion
    }
}