using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Utility
{
    internal class SqlTableData
    {
        #region Fields

        private IDictionary<String, Int32> _tableColumns;
        private Int32 _noOfColumns = -1;

        private Collection<Object[]> _tableRows;
        private Int32 _maxRowIndex = -1;
        private Int32 _currentRowIndex = -1;

        #endregion

        #region Constructor

        public SqlTableData()
        {
            _tableColumns = new Dictionary<String, Int32>();
            _tableRows = new Collection<Object[]>();
        }

        public SqlTableData(IDataReader dataReader) : this()
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
                _tableColumns.Add(dataReader.GetName(columnIndex), columnIndex);
            }
        }

        private void PopulateTableRows(IDataReader dataReader)
        {
            Object[] values = null;
            while (dataReader.Read())
            {
                values = new Object[_noOfColumns];
                dataReader.GetValues(values);
                _tableRows.Add(values);
            }
            _maxRowIndex = _tableRows.Count - 1;
        }

        #endregion

        #region Read Records

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
                    _tableRows[_currentRowIndex] = null;
                    _currentRowIndex++;
                    //_tableRows.RemoveAt(_currentRowIndex);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 FieldCount
        {
            get { return _noOfColumns; }
        }

        public int GetOrdinal(string name)
        {
            return _tableColumns[name];
        }

        public Object this[String name]
        {
            get
            {
                Object[] currentRow = _tableRows[_currentRowIndex];
                return currentRow.GetValue(_tableColumns[name]);
            }
        }

        public Object this[Int32 i]
        {
            get
            {
                Object[] currentRow = _tableRows[_currentRowIndex];
                return currentRow.GetValue(i);
            }
        }

        #endregion

        #endregion
    }
}
