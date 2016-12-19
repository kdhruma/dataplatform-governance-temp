using System;
using System.Collections.ObjectModel;

namespace MDM.BusinessObjects
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies WordElementReportSettings
    /// </summary>
    public class WordElementReportSettings : IWordElementReportSettings
    {
        #region Fields

        private Int32 _wordListId = -1;
        private Int32? _countFrom = null;
        private Int32? _countTo = null;
        private Collection<GenericSearchRule<WordElementColumn>> _wordElementsSearchColumns = new Collection<GenericSearchRule<WordElementColumn>>();
        private Collection<GenericSortRule<WordElementColumn>> _wordElementsSortColumns = new Collection<GenericSortRule<WordElementColumn>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Specifies parameterless constructor
        /// </summary>
        public WordElementReportSettings()
        {
        }

        /// <summary>
        /// Specifies WordElementReportSettings constructor
        /// </summary>
        public WordElementReportSettings(Int32 wordListId, Int32? countFrom, Int32? countTo,
            Collection<GenericSearchRule<WordElementColumn>> wordElementsSearchColumns, Collection<GenericSortRule<WordElementColumn>> wordElementsSortColumns)
        {
            _wordListId = wordListId;
            _countFrom = countFrom;
            _countTo = countTo;
            _wordElementsSearchColumns = wordElementsSearchColumns;
            _wordElementsSortColumns = wordElementsSortColumns;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Specifies parent Word List Id
        /// </summary>
        public Int32 WordListId
        {
            get { return _wordListId; }
            set { _wordListId = value; }
        }

        /// <summary>
        /// Specifies CountFrom parameter for paged result (including element with CountFrom index, starting from 1)
        /// </summary>
        public Int32? CountFrom
        {
            get { return _countFrom; }
            set { _countFrom = value; }
        }

        /// <summary>
        /// Specifies CountTo parameter for paged result (including element with CountTo index)
        /// </summary>
        public Int32? CountTo
        {
            get { return _countTo; }
            set { _countTo = value; }
        }

        /// <summary>
        /// Denotes filtering and sorting parameters for each required WordElement field
        /// </summary>
        public Collection<GenericSearchRule<WordElementColumn>> WordElementsSearchColumns
        {
            get { return _wordElementsSearchColumns; }
            set { _wordElementsSearchColumns = value; }
        }

        /// <summary>
        /// Denotes filtering and sorting parameters for each required WordElement field
        /// </summary>
        public Collection<GenericSortRule<WordElementColumn>> WordElementsSortColumns
        {
            get { return _wordElementsSortColumns; }
            set { _wordElementsSortColumns = value; }
        }

        #endregion

    }
}
