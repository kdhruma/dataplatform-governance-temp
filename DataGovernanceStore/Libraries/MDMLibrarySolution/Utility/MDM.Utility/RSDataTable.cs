using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Utility
{
    /// <summary>
    /// Class represents a data table that can be used for loading external data into memory
    /// </summary>
    internal class RSDataTable
    {
        #region Fields

        /// <summary>
        /// Field represents the columns of the table in a column name based dictionary
        /// </summary>
        private IDictionary<String, Int32> _columns;

        /// <summary>
        /// Field represents the rows of a table
        /// </summary>
        private Collection<Object[]> _rows;

        /// <summary>
        /// Field represents the no of columns present in the table
        /// </summary>
        private Int32 _noOfColumns = -1;

        /// <summary>
        /// Field represents the maximum no of row indexes available for the table
        /// </summary>
        private Int32 _maxRowIndex = -1;

        /// <summary>
        /// Field represents the current row index of the table
        /// </summary>
        private Int32 _currentRowIndex = -1;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize the members
        /// </summary>
        public RSDataTable()
        {
            _columns = new Dictionary<String, Int32>();
            _rows = new Collection<Object[]>();
        }

        /// <summary>
        /// Instantiates and fills the table using the specified reader
        /// </summary>
        public RSDataTable(IDataReader dataReader) : this()
        {
            PopulateTableColumns(dataReader);
            PopulateTableRows(dataReader);
        }
        
        #endregion

        #region Methods

        #region Populate Records

        private void PopulateTableColumns(IDataReader dataReader)
        {
            _noOfColumns = dataReader.FieldCount;
            for (int columnIndex = 0; columnIndex < _noOfColumns; columnIndex++)
            {
                _columns.Add(dataReader.GetName(columnIndex), columnIndex);
            }
        }

        private void PopulateTableRows(IDataReader dataReader)
        {
            Object[] values = null;
            while (dataReader.Read())
            {
                values = new Object[_noOfColumns];
                dataReader.GetValues(values);
                _rows.Add(values);
            }
            _maxRowIndex = _rows.Count - 1;
        }

        #endregion

        #region Read Records

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public Boolean Read()
        {
            if (_currentRowIndex < _maxRowIndex)
            {
                if (_currentRowIndex < 0)
                {
                    _currentRowIndex++;
                }
                else
                {
                    _rows[_currentRowIndex] = null;
                    _currentRowIndex++;                    
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the number of columns in the current row
        /// </summary>
        public Int32 FieldCount
        {
            get { return _noOfColumns; }
        }

        /// <summary>
        /// Gets the column ordinal based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Int32 GetOrdinal(string name)
        {
            return _columns[name];
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <param name="name">Represents the column name</param>
        /// <returns></returns>
        public Object this[String name]
        {
            get
            {
                Object[] currentRow = _rows[_currentRowIndex];
                return currentRow.GetValue(_columns[name]);
            }
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <param name="index">Represents the column index</param>
        /// <returns></returns>
        public Object this[Int32 index]
        {
            get
            {
                Object[] currentRow = _rows[_currentRowIndex];
                return currentRow.GetValue(index);
            }
        }

        /// <summary>
        /// Gets the name for the field to find
        /// </summary>
        public String GetName(Int32 index)
        {
            foreach (KeyValuePair<String, Int32> keyValuePair in _columns)
            {
                if (keyValuePair.Value == index)
                {
                    return keyValuePair.Key;
                }
            }
            return String.Empty;
        }

        #endregion

        #endregion
    }
}
