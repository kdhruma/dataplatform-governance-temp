using System;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for GenericSearchRule for searching
    /// </summary>
    public class GenericSearchRule<T> : IGenericSearchRule<T>
    {
        #region Fields

        private T _column;
        private SearchOperator _operator;
        private String _searchTerm;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public GenericSearchRule()
        {
            _column = default(T);
            _operator = SearchOperator.Like;
            _searchTerm = String.Empty;
        }

        /// <summary>
        /// Constructs GenericSearchRule object
        /// </summary>
        /// <param name="column"></param>
        /// <param name="searchOperator"></param>
        /// <param name="searchTerm"></param>
        public GenericSearchRule(T column, SearchOperator searchOperator, String searchTerm)
        {
            _column = column;
            _operator = searchOperator;
            _searchTerm = searchTerm;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies generic element column representation
        /// </summary>
        public T Column
        {
            get { return _column; }
            set { _column = value; }
        }

        /// <summary>
        /// Specifies search operator
        /// </summary>
        public SearchOperator Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }

        /// <summary>
        /// Specifies search term
        /// </summary>
        public String SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }

        #endregion
    }
}
