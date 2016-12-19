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
    /// Class implements IDataReader, by providing utilities to sequentially load data into memory and to read from them
    /// </summary>
    internal class RSDataReader : IDataReader
    {
        #region Fields

        /// <summary>
        /// Field represents all the result sets (tables) loaded in memory
        /// </summary>
        private Collection<RSDataTable> _tableCollection;

        /// <summary>
        /// Field represents the current index of the result set
        /// </summary>
        private Int32 _currentResultSetIndex = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize the members
        /// </summary>
        public RSDataReader()
        {
            _tableCollection = new Collection<RSDataTable>();
        }

        /// <summary>
        /// Instantiates and fills the RSDataReader based on the specified IDataReader
        /// </summary>
        /// <param name="dataReader">The data reader which provides the data to be loaded in memory</param>
        public RSDataReader(IDataReader dataReader) : this()
        {
            using (dataReader)
            {
                RSDataTable tableData = null;
                do
                {
                    tableData = new RSDataTable(dataReader);
                    _tableCollection.Add(tableData);
                }
                while (dataReader.NextResult());
            }
        }

        #endregion

        #region IDataReader Methods

        #region Implemented Methods

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public Boolean Read()
        {
            return _tableCollection[_currentResultSetIndex].Read();
        }

        /// <summary>
        /// Gets the number of columns in the current row
        /// </summary>
        public Int32 FieldCount
        {
            get { return _tableCollection[_currentResultSetIndex].FieldCount; }
        }

        /// <summary>
        /// Gets the column ordinal based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Int32 GetOrdinal(String name)
        {
            return _tableCollection[_currentResultSetIndex].GetOrdinal(name);
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
                return _tableCollection[_currentResultSetIndex][name];
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
                return _tableCollection[_currentResultSetIndex][index];
            }
        }

        /// <summary>
        /// Advances the reader to the next result if result sets are available.
        /// </summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public Boolean NextResult()
        {
            if (_currentResultSetIndex < (_tableCollection.Count - 1))
            {
                _tableCollection[_currentResultSetIndex] = null;
                _currentResultSetIndex++;                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name for the field to find
        /// </summary>
        public String GetName(Int32 index)
        {
            return _tableCollection[_currentResultSetIndex].GetName(index);
        }

        #endregion

        #region NotImplemented Methods

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int32 Depth
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Boolean IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int32 RecordsAffected
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Boolean GetBoolean(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Byte GetByte(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int64 GetBytes(Int32 i, Int64 fieldOffset, Byte[] buffer, Int32 bufferoffset, Int32 length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Char GetChar(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int64 GetChars(Int32 i, Int64 fieldoffset, char[] buffer, Int32 bufferoffset, Int32 length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public IDataReader GetData(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public String GetDataTypeName(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public DateTime GetDateTime(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Decimal GetDecimal(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Double GetDouble(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Type GetFieldType(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public float GetFloat(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Guid GetGuid(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int16 GetInt16(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int32 GetInt32(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int64 GetInt64(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public String GetString(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Object GetValue(Int32 i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Int32 GetValues(Object[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To be implemented as and when required
        /// </summary>
        public Boolean IsDBNull(Int32 i)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}
