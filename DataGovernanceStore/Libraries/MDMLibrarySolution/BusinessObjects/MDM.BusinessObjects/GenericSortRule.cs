using System;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Specifies class for GenericSortRule for searching
    /// </summary>
    public class GenericSortRule<T> : IGenericSortRule<T>
    {
        #region Fields

        private T _column;
        private Boolean _isDescendingOrder;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public GenericSortRule()
        {
            _column = default(T);
            _isDescendingOrder = false;
        }

        /// <summary>
        /// Constructs GenericSortRule object
        /// </summary>
        /// <param name="column"></param>
        /// <param name="isDescendingOrder"></param>
        public GenericSortRule(T column, Boolean isDescendingOrder)
        {
            _column = column;
            _isDescendingOrder = isDescendingOrder;
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
        /// Specifies order of sorting
        /// </summary>
        public Boolean IsDescendingOrder
        {
            get { return _isDescendingOrder; }
            set { _isDescendingOrder = value; }
        }

        #endregion

    }
}
