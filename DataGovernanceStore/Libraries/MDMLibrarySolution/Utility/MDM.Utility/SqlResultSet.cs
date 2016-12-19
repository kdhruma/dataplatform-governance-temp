using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Utility
{
    internal class SqlResultSet : IDataReader
    {
        #region Fields

        private Collection<SqlTableData> _resultSet;
        private Int32 _currentResultSetIndex = 0;

        #endregion

        #region Constructor

        public SqlResultSet()
        {
            _resultSet = new Collection<SqlTableData>();
        }

        public void Load(IDataReader dataReader)
        {
            using (dataReader)
            {
                SqlTableData tableData = null;
                do
                {
                    tableData = new SqlTableData(dataReader);
                    _resultSet.Add(tableData);
                }
                while (dataReader.NextResult());
            }
        }

        #endregion

        #region IDataReader Methods

        #region Implemented Methods

        public Boolean Read()
        {
            return _resultSet[_currentResultSetIndex].Read();
        }

        public Int32 FieldCount
        {
            get { return _resultSet[_currentResultSetIndex].FieldCount; }
        }

        public Int32 GetOrdinal(String name)
        {
            return _resultSet[_currentResultSetIndex].GetOrdinal(name);
        }

        public Object this[String name]
        {
            get
            {
                return _resultSet[_currentResultSetIndex][name];
            }
        }

        public Object this[Int32 index]
        {
            get
            {
                return _resultSet[_currentResultSetIndex][index];
            }
        }

        public Boolean NextResult()
        {
            if (_currentResultSetIndex < (_resultSet.Count - 1))
            {
                _resultSet[_currentResultSetIndex] = null;
                _currentResultSetIndex++;
                //_resultSet.RemoveAt(_currentResultIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region NotImplemented Methods

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public int RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
